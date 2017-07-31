using MvvmHelpers;
using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.Services;
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
    public class CreateSharingSpaceBViewModel : BaseViewModel
    {
        ApiServices _apiServices = new ApiServices();
        public SharingSpace sharingSpace { get; set; }
        public List<DimensionLocal> dimensions { get; set; }

        public CreateSharingSpaceBViewModel() 
        {
            
        }

        public CreateSharingSpaceBViewModel(SharingSpace sharingSpace, List<DimensionLocal> dimensions)
        {
            this.sharingSpace = sharingSpace;
            this.dimensions = dimensions;
            ReloadTags();
            RemoveTagCommand = new BaseCommand<TagItem>((arg) => RemoveTag(arg));
        }

        private ObservableCollection<TagItem> items;

        public ObservableCollection<TagItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        private bool isReallyBusy;
        public bool IsReallyBusy
        {
            get { return isReallyBusy; }
            set { SetProperty(ref isReallyBusy, value); }
        }

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

            if (Items.Any(v => v.Name.Equals(tagString, StringComparison.OrdinalIgnoreCase)))
                return null;

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
            try
            {
                IsBusy = true;
                var response = await _apiServices.PostSharingSpaceAsync(this.sharingSpace, Settings.AccessToken);

                foreach (var dimension in this.dimensions)
                {
                    var dimensionId = Utilities.NewGuid();
                    var dataypeId = Utilities.NewGuid();
                    await _apiServices.PostDimensionAsync(new Dimension { Id = dimensionId, Interval = dimension.Interval, Label = dimension.Label });
                    await _apiServices.PostDatatypeAsync(new Datatype { Id = dataypeId, Type = "string", Domain = "" });
                    await _apiServices.PostDimDatatypeAsync(new DimDatatype { DatatypeId = dataypeId, DimensionId = dimensionId});
                    foreach (var constraint in dimension.ConstraintList)
                    {
                        constraint.Id = Utilities.NewGuid();
                        await _apiServices.PostConstraintAsync(constraint);
                        await _apiServices.PostEventAsync(new Event { ConstraintId = constraint.Id, DimensionId = dimensionId, SharingSpaceId = this.sharingSpace.Id });
                    }
                }
               // navigate to event detail (overview) with sharing space in constructor
               //Application.Current.MainPage.Navigation.PushAsync(new Views.Event(selectedItem));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Error Full: " + ex);
                await Application.Current.MainPage.DisplayAlert("Event creation failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = true;
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


