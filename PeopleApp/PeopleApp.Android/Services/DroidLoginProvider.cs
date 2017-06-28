using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Abstractions;
using PeopleApp.Droid.Services;
using PeopleApp.Helpers;
//using Xamarin.Auth;

[assembly: Xamarin.Forms.Dependency(typeof(DroidLoginProvider))]
namespace PeopleApp.Droid.Services
{
    public class DroidLoginProvider : ILoginProvider
    {
        Context context;

        public void Init(Context context)
        {
            this.context = context;
        }

        //public Task<MobileServiceUser> LoginAsync(MobileServiceClient client)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client)
        {
            return await client.LoginAsync(context, "facebook");
        }
    }
}