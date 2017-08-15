using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeopleApp.Services;
using PeopleApp.Models;
using System.Diagnostics;
using PeopleApp.Abstractions;

namespace PeopleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventLoadingPage : ContentPage
    {
        ApiServices _apiServices = new ApiServices();
        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();

        public EventLoadingPage()
        {
            //NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar

            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ICollection<SharingSpace> sharingSpaces = null;
            var sharingSpaceTable = await CloudService.GetTableAsync<SharingSpace>();

            // checks if there is already a selected sharing space if not the history page will appear
            if (!String.IsNullOrEmpty(Settings.CurrentSharingSpace))
            {
                //var sharingSpace = await CloudService.GetSharingSpace(Settings.CurrentSharingSpace);
                SharingSpace sharingSpace = await sharingSpaceTable.ReadItemAsync(Settings.CurrentSharingSpace);
                var objectList = await _apiServices.GetObjectsBySharingSpace(sharingSpace.Id);
                Navigation.InsertPageBefore(new EventOverviewPage(sharingSpace, objectList), this);
                await Navigation.PopAsync();
                return;
                //await Navigation.PushAsync(new EventOverviewPage());
            }

            try
            {
                //sharingSpaces = await _apiServices.GetSharingSpaceAsync(Settings.AccessToken);
                sharingSpaces = await sharingSpaceTable.ReadAllItemsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskDetail] Load error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Load Events Failed", ex.Message, "OK");
            }

            // checks if there is already a selected sharing space if not the history page will appear
            //if (!String.IsNullOrEmpty(Settings.CurrentSharingSpace))
            //{
            //    //var sharingSpace = await CloudService.GetSharingSpace(Settings.CurrentSharingSpace);
            //    SharingSpace sharingSpace = await sharingSpaceTable.ReadItemAsync(Settings.CurrentSharingSpace);
            //    Navigation.InsertPageBefore(new EventOverviewPage(sharingSpace), this);
            //    await Navigation.PopAsync();
            //    //await Navigation.PushAsync(new EventOverviewPage());
            //}
            // else if there is events list them otherwise open no event page
            if (sharingSpaces.Count > 0)
            {
                try
                {
                    //Navigation.InsertPageBefore(new EventList(sharingSpaces.ToList()), this);
                    Navigation.InsertPageBefore(new EventHistoryPage(), this);
                    await Navigation.PopAsync();
                    //await Navigation.PushAsync(new EventList(sharingSpaces));

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Load error: {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Couldn't load page", ex.Message, "OK");
                }

            }
            else
            {
                //Add NoExistingEventsPage (You do not have any events yet. Do you want to create now...)
                Navigation.InsertPageBefore(new CreateSharingSpaceAPage(), this);
                await Navigation.PopAsync();
                //await Navigation.PushAsync(new CreateSharingSpaceAPage());

            }

        }
    }
}