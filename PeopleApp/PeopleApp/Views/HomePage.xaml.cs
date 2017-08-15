using Plugin.Connectivity;
using System;
using PeopleApp.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
            //NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
            welcomeText.Text = "Hello " + Settings.Username + " !";
        }

        private async void CreateSharingSpace_Clicked(object sender, EventArgs e)
        {
            if (!(await CrossConnectivity.Current.IsRemoteReachable(Constants.BaseApiAddress, 443)))
            {
                await Application.Current.MainPage.DisplayAlert("Action denied", "You are not connected to the internet. Therefore, it is impossible for you to create a new event. Please get connected!", "OK");
                return;
            }

            await Navigation.PushAsync(new CreateSharingSpaceAPage());
        }

        private async void JoinSharingSpace_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EventList());
        }
    }
}