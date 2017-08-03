using PeopleApp.Models.ViewsRelated;
using PeopleApp.Services;
using PeopleApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeopleApp.ViewModels;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        ApiServices _apiServices = new ApiServices();
        //private UserInfo userInfo;

        public ProfilePage(UserInfo userInfo)
        {
            InitializeComponent();
            //this.userInfo = userInfo;
            username.Text = userInfo.UserId;
            // load user profile on application launch then activate this
            //Settings.Username = userInfo.UserId;
            firstname.Text = userInfo.UserClaims.Where(item => item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")).FirstOrDefault<UserClaim>().Value;
            lastname.Text = userInfo.UserClaims.Where(item => item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).FirstOrDefault<UserClaim>().Value;
            email.Text = userInfo.UserClaims.Where(item => item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress") || item.Type.Equals("emails")).FirstOrDefault<UserClaim>().Value;
            //gender.Text = userInfo.UserClaims.Where(item => item.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender")).FirstOrDefault<UserClaim>().Value;


        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    List<UserInfo> user = await _apiServices.GetUserInfoAsync(Settings.AccessToken);

        //}

        public override string ToString()
        {
            return this.Title;
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {

        }
    }
}
