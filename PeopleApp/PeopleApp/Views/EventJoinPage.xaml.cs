using PeopleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeopleApp.Models;
using PeopleApp.Models.ViewsRelated;
using System.Diagnostics;
using PeopleApp.Services;
using PeopleApp.Helpers;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventJoinPage : ContentPage
	{
        ApiServices _apiServices = new ApiServices();
        private List<SharingSpace> sharingSpaces;
        private SharingSpaceView _oldItem;
        private SharingSpace selectedSharingSpace;

        public EventJoinPage(List<SharingSpace> sharingSpaces)
        {
            this.sharingSpaces = sharingSpaces ?? throw new ArgumentNullException();

            InitializeComponent();
            // BindingContext = new EventListViewModel(sharingSpaces);
            BindingContext = new EventJoinViewModel(sharingSpaces);
            //MessagingCenter.Subscribe<EventJoinViewModel, SharingSpace>(this, "SelectJoinEvent", OnSelectedSharingSpace);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = BindingContext as EventJoinViewModel;
            
            var product = e.Item as SharingSpaceView;
            selectedSharingSpace = vm.HideOrShowProduct(product);
        }

        private async void Join_Clicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                // check if he belongs already to this event or he is the owner
                //var table = await CloudService.GetTableAsync<SharingSpace>();
                var res1 = await _apiServices.VerifyOnwershipOfEvent(selectedSharingSpace.Id, Settings.AccessToken);
                var res2 = await _apiServices.VerifyUserParticipation(selectedSharingSpace.Id, Settings.AccessToken);
                if (res1 == null && res2 == null)
                {
                    // not owner neither participant
                    // add constraint
                    var constraintId = Utilities.NewGuid();
                    Models.Constraint socialConstraint = new Models.Constraint { Id = constraintId, Operator = "participant", Value = Settings.UserId };
                    await _apiServices.PostConstraintAsync(socialConstraint);
                    // get social dimensionId related to the current sharing space
                    string dimensionId = await _apiServices.GetDimensionId(selectedSharingSpace.Id, "Social", Settings.AccessToken);
                    Event newEvent = new Event { ConstraintId = constraintId, DimensionId = dimensionId, SharingSpaceId = selectedSharingSpace.Id };
                    await _apiServices.PostEventAsync(newEvent);

                    // Navigate to event overview
                    var objectList = await _apiServices.GetObjectsBySharingSpace(selectedSharingSpace.Id);
                    Navigation.InsertPageBefore(new EventOverviewPage(selectedSharingSpace, objectList), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    // Navigate to event overview
                    var objectList = await _apiServices.GetObjectsBySharingSpace(selectedSharingSpace.Id);
                    //MenuPage.NavPage = new EventOverviewPage(selectedSharingSpace, objectList);
                    Navigation.InsertPageBefore(new EventOverviewPage(selectedSharingSpace, objectList), this);
                    await Navigation.PopAsync();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        //private async void OnSelectedSharingSpace(EventJoinViewModel source, SharingSpace sharingSpace)
        //{
        //    try
        //    {
        //        var objectList = await _apiServices.GetObjectsBySharingSpace(sharingSpace.Id);
        //        Navigation.InsertPageBefore(new EventOverviewPage(sharingSpace, objectList), this);
        //        await Navigation.PopAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Error :" + ex.Message);
        //    }

        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    MessagingCenter.Unsubscribe<EventJoinViewModel, SharingSpace>(this, "SelectJoinEvent");// .Subscribe<EventHistoryViewModel, SharingSpace>(this, "SelectEvent", OnSelectedSharingSpace);
        //}
    }
}
