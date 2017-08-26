using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.Services;
using Plugin.Geolocator;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeopleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinEventLoadingPage : ContentPage
    {
        ApiServices _apiServices = new ApiServices();
        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ICloudTable<User> userTable;

        public JoinEventLoadingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            userTable = await CloudService.GetTableAsync<User>();
            User currentUser = await userTable.ReadItemAsync(Settings.UserId);

            await UpdateUserLocation(currentUser);

            try
            {
                List<SharingSpace> sharingspaces = await _apiServices.GetNearbyEvents(Settings.AccessToken, "500");
                if(sharingspaces == null)
                {
                    await Application.Current.MainPage.DisplayAlert("No nearby events", "No nearby events.", "OK");
                    Navigation.InsertPageBefore(new HomePage(), this);
                    await Navigation.PopAsync();
                }
                Navigation.InsertPageBefore(new EventJoinPage(sharingspaces), this);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                await Application.Current.MainPage.DisplayAlert("Getting events failed", ex.Message, "OK");
                // do nothing
            }




        }

        private async Task UpdateUserLocation(User user)
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 25;

            try
            {

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(15));
                if (position != null)
                {
                    //userTable = await CloudService.GetTableAsync<User>();
                    CultureInfo culture = CultureInfo.CurrentCulture;
                    user.Longitude = position.Longitude; user.Latitude = position.Latitude; user.CultureInfo = culture.ToString();
                    await userTable.UpdateItemAsync(user);
                    await CloudService.SyncOfflineCacheAsync();
                }


            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine("Error " + e.Message);
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + e);
                var answer = await Application.Current.MainPage.DisplayAlert("Detecting location error", "Please turn on GPS. Would you like to try again?", "Yes", "No");
                // do nothing
                Debug.WriteLine("Answer: " + answer);
                if (answer.Equals("Yes"))
                {
                    Navigation.InsertPageBefore(new JoinEventLoadingPage(), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    Navigation.InsertPageBefore(new HomePage(), this);
                    await Navigation.PopAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error " + e.Message);
                Debug.WriteLine("Unable to get address at this moment!");
                var answer = await Application.Current.MainPage.DisplayAlert("Detecting location error", "Please turn on GPS. Would you like to try again?", "Yes", "No");
                // do nothing
                Debug.WriteLine("Answer: " + answer);
                if (answer.Equals("Yes"))
                {
                    Navigation.InsertPageBefore(new JoinEventLoadingPage(), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    Navigation.InsertPageBefore(new HomePage(), this);
                    await Navigation.PopAsync();
                }
            }


        }
    }
}