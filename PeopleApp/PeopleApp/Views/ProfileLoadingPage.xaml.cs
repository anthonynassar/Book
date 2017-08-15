using PeopleApp.Services;
using System;
using PeopleApp.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using PeopleApp.Models.ViewsRelated;
using PeopleApp.Abstractions;
using PeopleApp.Models;

namespace PeopleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileLoadingPage : ContentPage
    {
        ApiServices _apiServices = new ApiServices();
        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();

        public ProfileLoadingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //List<AppServiceIdentity> user = new List<AppServiceIdentity>();
            // get user info
            try
            {
                var table = await CloudService.GetTableAsync<User>();
                User user = await table.ReadItemAsync(Settings.UserId);
                //user = await _apiServices.GetUserInfoAsync(Settings.AccessToken);

                // navigate to user page populated with these user info
                //AppServiceIdentity userInfo = user.FirstOrDefault<AppServiceIdentity>();
                Navigation.InsertPageBefore(new ProfilePage(user), this);
                await Navigation.PopAsync();
            }
            catch (Exception e1)
            {
                Debug.WriteLine($"[TaskDetail] Load error: {e1.Message}");
                await Application.Current.MainPage.DisplayAlert("Load User Profile Failed", e1.Message, "OK");
                try
                {
                    Navigation.InsertPageBefore(new HomePage(), this);
                    await Navigation.PopAsync();
                }
                catch (Exception e2)
                {
                    await Application.Current.MainPage.DisplayAlert("Navigation Failed", e2.Message, "OK");
                }
                
            }
        }
    }
}