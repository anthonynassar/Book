using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Media;
using Java.IO;
using Android.Content;
using System.Collections.Generic;
using Android.Provider;
using PeopleApp.Droid.Services;
using Xamarin.Forms;
using PeopleApp.Abstractions;
using PeopleApp.Droid.Renderers;

namespace PeopleApp.Droid
{
	[Activity (Label = "SpiderApp", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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
            TagEntryRenderer.Init();

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
            }

            LoadApplication(new PeopleApp.App ());
		}
        private void CreateDirectoryForPictures()
        {
            App._dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "PeopleApp");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
            PeopleApp.Helpers.Settings.PhotoAlbumPath = App._dir.AbsolutePath;
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
    }
    public static class App
    {
        public static File _file;
        public static File _dir;
    }
}
