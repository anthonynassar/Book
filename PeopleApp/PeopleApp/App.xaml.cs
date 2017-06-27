using PeopleApp.Abstractions;
using PeopleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PeopleApp
{
	public partial class App : Application
	{
        public static ICloudService CloudService { get; set; }

        public App ()
		{
			InitializeComponent();

            CloudService = new AzureCloudService();
            MainPage = new PeopleApp.MainPage();
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
