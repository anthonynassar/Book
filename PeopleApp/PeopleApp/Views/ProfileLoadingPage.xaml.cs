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

namespace PeopleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileLoadingPage : ContentPage
    {
        ApiServices _apiServices = new ApiServices();

        public ProfileLoadingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            List<UserInfo> user = new List<UserInfo>();
            // get user info
            try
            {
                user = await _apiServices.GetUserInfoAsync(Settings.AccessToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskDetail] Load error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Load User Profile Failed", ex.Message, "OK");
            }

            // navigate to user page populated with these user info
            UserInfo userInfo = user.FirstOrDefault<UserInfo>();
            Navigation.InsertPageBefore(new ProfilePage(userInfo), this);
            await Navigation.PopAsync();
        }
    }
}