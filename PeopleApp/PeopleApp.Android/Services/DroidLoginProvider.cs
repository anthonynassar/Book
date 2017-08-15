using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using PeopleApp.Abstractions;
using PeopleApp.Droid.Services;
using PeopleApp.Helpers;
using Xamarin.Auth;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(DroidLoginProvider))]
namespace PeopleApp.Droid.Services
{
    public class DroidLoginProvider : ILoginProvider
    {
        #region ILoginProvider Interface
        public MobileServiceUser RetrieveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService("tasklist");
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    string token;

                    if (acct.Properties.TryGetValue("token", out token))
                    {
                        return new MobileServiceUser(acct.Username)
                        {
                            MobileServiceAuthenticationToken = token
                        };
                    }
                }
            }
            return null;
        }

        public void StoreTokenInSecureStore(MobileServiceUser user)
        {
            var account = new Account(user.UserId);
            account.Properties.Add("token", user.MobileServiceAuthenticationToken);
            AccountStore.Save(account, "tasklist");
        }

        //public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client)
        //{
        //    // Server Flow
        //    return await client.LoginAsync(RootView, "facebook");
        //}
        #endregion

        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, string provider)
        {
            // Server Flow
            return await client.LoginAsync(RootView, provider);
        }

        public Context RootView { get; private set; }

        public AccountStore AccountStore { get; private set; }

        public void Init(Context context)
        {
            RootView = context;
            AccountStore = AccountStore.Create(context);
        }

        public void RemoveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService("tasklist");
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    AccountStore.Delete(acct, "tasklist");
                }
            }
        }

        public string GetSyncStore()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "syncstore.db");

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            return path;
        }

        //async Task ILoginProvider.LoginAsync(MobileServiceClient client)
        //{
        //    await client.LoginAsync(RootView, "aad");
        //}
    }
}