using System;
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
            welcomeText.Text = "Hello USER!!";
        }

        private async void CreateSharingSpace_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateSharingSpaceAPage());
        }

        private async void JoinSharingSpace_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EventList());
        }
    }
}