using MvvmHelpers;
using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.Services;
using PeopleApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamvvm;

namespace PeopleApp.ViewModels
{
    public class CreateSharingSpaceBViewModel : MvvmHelpers.BaseViewModel
    {
        ApiServices _apiServices = new ApiServices();
        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public SharingSpace SharingSpace { get; set; }
        public List<DimensionView> Dimensions { get; set; }
        private ContentPage _page;

        public CreateSharingSpaceBViewModel() 
        {
            //ReloadTags();
            //RemoveTagCommand = new BaseCommand<TagItem>((arg) => RemoveTag(arg));
        }

        public CreateSharingSpaceBViewModel(SharingSpace sharingSpace, List<DimensionView> dimensions, CreateSharingSpaceBPage page)
        {
            this.SharingSpace = sharingSpace;
            this.Dimensions = dimensions;
            _page = page;
            ReloadTags();
            RemoveTagCommand = new BaseCommand<TagItem>((arg) => RemoveTag(arg));
        }

        private ObservableCollection<TagItem> items;

        public ObservableCollection<TagItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        //private bool isDone;
        //public bool IsDone
        //{
        //    get { return isDone; }
        //    set { SetProperty(ref isDone, value); }
        //}

        //string loadingMessage;
        //public string LoadingMessage
        //{
        //    get { return loadingMessage; }
        //    set { SetProperty(ref loadingMessage, value); }
        //}

        public void ReloadTags()
        {
            var tags = new ObservableCollection<TagItem>(){
                new TagItem() { Name = "#TagExample" },
                new TagItem() { Name = "#Xamarin" },
                new TagItem() { Name = "#AnthonyNassar" },
                new TagItem() { Name = "#Test" },
                new TagItem() { Name = "#XamarinForms" },
                new TagItem() { Name = "#TagEntryView" },
                new TagItem() { Name = "#TapMe!" },
                new TagItem() { Name = "#itsworking!" },
            };

            Items = tags;
        }

        public void RemoveTag(TagItem tagItem)
        {
            if (tagItem == null)
                return;

            Items.Remove(tagItem);
        }

        public TagItem ValidateAndReturn(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return null;

            var tagString = tag.StartsWith("#") ? tag : "#" + tag;

            try
            {
                if (Items.Any(v => v.Name.Equals(tagString, StringComparison.OrdinalIgnoreCase)))
                    return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Error Full: " + ex);
                return null;
            }

            

            var newTag = new TagItem()
            {
                Name = tagString.ToLower()
            };

            //Items.Add(newTag);

            return newTag;
        }

        ICommand createEventCommand;
        public ICommand CreateEventCommand =>
            createEventCommand ?? (createEventCommand = new Command(async () => await ExecuteCreateEventCommandAsync()));

        async Task ExecuteCreateEventCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Models.Constraint socialConstraint = new Models.Constraint { Operator = "owner", Value = Settings.UserId };
                DimensionView socialDimension = new DimensionView { Label = "Social", Interval = true, ConstraintList = new List<Models.Constraint> { socialConstraint } };
                Dimensions.Add(socialDimension);

                Settings.CurrentSharingSpace = this.SharingSpace.Id;
                var response = await _apiServices.PostSharingSpaceAsync(this.SharingSpace, Settings.AccessToken);

                foreach (var dimension in this.Dimensions)
                {
                    var dimensionId = Utilities.NewGuid();
                    var dataypeId = Utilities.NewGuid();
                    await _apiServices.PostDimensionAsync(new Dimension { Id = dimensionId, Interval = dimension.Interval, Label = dimension.Label });
                    // metadata related tables
                    await _apiServices.PostDatatypeAsync(new Datatype { Id = dataypeId, Type = "string", Domain = "" });
                    await _apiServices.PostDimDatatypeAsync(new DimDatatype { DatatypeId = dataypeId, DimensionId = dimensionId });
                    foreach (var constraint in dimension.ConstraintList)
                    {
                        constraint.Id = Utilities.NewGuid();
                        await _apiServices.PostConstraintAsync(constraint);
                        await _apiServices.PostEventAsync(new Event { ConstraintId = constraint.Id, DimensionId = dimensionId, SharingSpaceId = this.SharingSpace.Id });
                    }
                }

                await CloudService.SyncOfflineCacheAsync();
                // navigate to event detail (overview)
                MessagingCenter.Send(this, "NavigateToEventOverview", SharingSpace);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Error Full: " + ex);
                await Application.Current.MainPage.DisplayAlert("Event creation failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        IBaseCommand removeTagCommand;
        public IBaseCommand RemoveTagCommand
        {
            get { return removeTagCommand; }
            set { SetProperty(ref removeTagCommand, value); }
        }

        public class TagItem : BaseModel
        {
            string name;
            public string Name
            {
                get { return name; }
                set { SetField(ref name, value); }
            }
        }
    }
}


