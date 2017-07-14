using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Services;
using PeopleApp.ViewModels;
using PeopleApp.Views;

using Xamarin.Forms;

namespace PeopleApp
{
	public partial class App : Application
	{
        public static ICloudService CloudService { get; set; }

        public App ()
		{
			InitializeComponent();

            //CloudService = new AzureCloudService();
            ServiceLocator.Instance.Add<ICloudService, AzureCloudService>();

            SetMainPage();
            //MainPage = new NavigationPage(new Views.EntryPage());
        }

        private void SetMainPage()
        {
            if (!string.IsNullOrEmpty(Settings.AccessToken))
            {
                if (AzureCloudService.IsTokenExpired(Settings.AccessToken))
                {
                    var vm = new EntryPageViewModel();
                    vm.LoginCommand.Execute(null);
                }
                MainPage = new NavigationPage(new TaskList());
            }

            //else if (!string.IsNullOrEmpty(Settings.Username) && !string.IsNullOrEmpty(Settings.Password))
            //    MainPage = new NavigationPage(new LoginPage());
            else
                MainPage = new NavigationPage(new EntryPage());
        }


        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
            // Handle when your app resumes
        }
	}
}
