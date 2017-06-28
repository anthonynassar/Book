using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Abstractions;
using PeopleApp.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(iOSLoginProvider))]
namespace PeopleApp.iOS.Services
{
    public class iOSLoginProvider : ILoginProvider
    {
        //public async Task<MobileServiceClient> LoginAsync(MobileServiceClient client)
        //{
        //    return result =  await client.LoginAsync(RootView, "facebook");
        //}

        async Task<MobileServiceUser> ILoginProvider.LoginAsync(MobileServiceClient client)
        {
            return await client.LoginAsync(RootView, "facebook");
        }

        public UIViewController RootView => UIApplication.SharedApplication.KeyWindow.RootViewController;
    }
}