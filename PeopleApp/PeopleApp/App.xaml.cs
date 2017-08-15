using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Services;
using PeopleApp.ViewModels;
using PeopleApp.Views;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;
using System;
using PeopleApp.Models;

namespace PeopleApp
{
	public partial class App : Application
	{
        //public static ICloudService CloudService { get; set; }
        //public static NavigationPage NavPage { get; private set; }
        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();

        public App ()
		{
			InitializeComponent();

            FlowListView.Init();
            //CloudService = new AzureCloudService();
            ServiceLocator.Add<ICloudService, AzureCloudService>();

            //LoadSomeData();
            SetMainPage();
            
            //MainPage = new NavigationPage(new Views.EntryPage());
        }

        //private async void LoadSomeData()
        //{
        //    if (!string.IsNullOrEmpty(Settings.AccessToken) && !string.IsNullOrEmpty(Settings.UserId))
        //    {
        //        var table = await CloudService.GetTableAsync<User>();
        //        User user = await table.ReadItemAsync(Settings.UserId);
        //        Settings.Username = user.Username;
        //    }
               
        //}

        private void SetMainPage()
        {
            if (!string.IsNullOrEmpty(Settings.AccessToken))
            {
                if (AzureCloudService.IsTokenExpired(Settings.AccessToken))
                {
                    var vm = new EntryPageViewModel();
                    vm.LoginCommand.Execute(null);
                }
                //NavPage = new NavigationPage(new MenuPage());
                //MainPage = NavPage;
                MainPage = new MenuPage();
            }

            //else if (!string.IsNullOrEmpty(Settings.Username) && !string.IsNullOrEmpty(Settings.Password))
            //    MainPage = new NavigationPage(new LoginPage());
            else
                MainPage = new EntryPage();
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
