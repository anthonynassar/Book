using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PeopleApp.Droid.Services;
using Xamarin.Forms;
using PeopleApp.Abstractions;

namespace PeopleApp.Droid
{
	[Activity (Label = "PeopleApp", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

            // useless for Android project
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            global::Xamarin.Forms.Forms.Init (this, bundle);

            var loginProvider = (DroidLoginProvider)DependencyService.Get<ILoginProvider>();
            loginProvider.Init(this);

            LoadApplication(new PeopleApp.App ());
		}
	}
}

