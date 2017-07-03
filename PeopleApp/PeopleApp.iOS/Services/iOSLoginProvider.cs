
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.iOS.Services;
using UIKit;
using Xamarin.Auth;
using PeopleApp.Services;

[assembly: Xamarin.Forms.Dependency(typeof(iOSLoginProvider))]
namespace PeopleApp.iOS.Services
{
    public class iOSLoginProvider : ILoginProvider
    {
        //public async Task<MobileServiceClient> LoginAsync(MobileServiceClient client)
        //{
        //    return result =  await client.LoginAsync(RootView, "facebook");
        //}

        public UIViewController RootView => UIApplication.SharedApplication.KeyWindow.RootViewController;

        public AccountStore AccountStore { get; private set; }

        public iOSLoginProvider()
        {
            AccountStore = AccountStore.Create();
        }

        public async Task LoginAsync(MobileServiceClient client)
        {
            // Check if the token is available within the key store
            var accounts = AccountStore.FindAccountsForService("tasklist");
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    string token;

                    if (acct.Properties.TryGetValue("token", out token))
                    {
                        if (!AzureCloudService.IsTokenExpired(token))
                        {
                            client.CurrentUser = new MobileServiceUser(acct.Username);
                            client.CurrentUser.MobileServiceAuthenticationToken = token;
                            return;
                        }
                    }
                }
            }

            // Server Flow
            await client.LoginAsync(RootView, "facebook");

            // Store the new token within the store
            var account = new Account(client.CurrentUser.UserId);
            account.Properties.Add("token", client.CurrentUser.MobileServiceAuthenticationToken);
            AccountStore.Save(account, "tasklist");
        }

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

    }
}