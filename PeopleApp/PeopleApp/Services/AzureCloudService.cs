﻿using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Abstractions;
using Xamarin.Forms;
using PeopleApp.Helpers;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http;

namespace PeopleApp.Services
{
    public class AzureCloudService : ICloudService
    {
        MobileServiceClient client;

        public AzureCloudService()
        {
            client = new MobileServiceClient(Locations.AppServiceUrl, new AuthenticationDelegatingHandler());

            if (Locations.AlternateLoginHost != null)
                client.AlternateLoginHost = new Uri(Locations.AlternateLoginHost);
        }

        public ICloudTable<T> GetTable<T>() where T : TableData => new AzureCloudTable<T>(client);

        public async Task<MobileServiceUser> LoginAsync(string provider)
        {
            var loginProvider = DependencyService.Get<ILoginProvider>();

            client.CurrentUser = loginProvider.RetrieveTokenFromSecureStore();
            if (client.CurrentUser != null)
            {
                // User has previously been authenticated - try to Refresh the token
                try
                {
                    var refreshed = await client.RefreshUserAsync();
                    if (refreshed != null)
                    {
                        loginProvider.StoreTokenInSecureStore(refreshed);
                        return refreshed;
                    }
                }
                catch (Exception refreshException)
                {
                    Debug.WriteLine($"Could not refresh token: {refreshException.Message}");
                }
            }

            if (client.CurrentUser != null && !IsTokenExpired(client.CurrentUser.MobileServiceAuthenticationToken))
            {
                // User has previously been authenticated, no refresh is required
                return client.CurrentUser;
            }

            // We need to ask for credentials at this point
            await loginProvider.LoginAsync(client, provider);
            if (client.CurrentUser != null)
            {
                // We were able to successfully log in
                loginProvider.StoreTokenInSecureStore(client.CurrentUser);
            }
            
            return client.CurrentUser;
        }

        public static bool IsTokenExpired(string token)
        {
            // Get just the JWT part of the token (without the signature).
            var jwt = token.Split(new Char[] { '.' })[1];

            // Undo the URL encoding.
            jwt = jwt.Replace('-', '+').Replace('_', '/');
            switch (jwt.Length % 4)
            {
                case 0: break;
                case 2: jwt += "=="; break;
                case 3: jwt += "="; break;
                default:
                    throw new ArgumentException("The token is not a valid Base64 string.");
            }

            // Convert to a JSON String
            var bytes = Convert.FromBase64String(jwt);
            string jsonString = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            // Parse as JSON object and get the exp field value,
            // which is the expiration date as a JavaScript primative date.
            JObject jsonObj = JObject.Parse(jsonString);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

            // Calculate the expiration by adding the exp value (in seconds) to the
            // base date of 1/1/1970.
            DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expire = minTime.AddSeconds(exp);
            return (expire < DateTime.UtcNow);
        }

        public async Task LogoutAsync()
        {
            if (client.CurrentUser == null || client.CurrentUser.MobileServiceAuthenticationToken == null)
                return;

            // Log out of the identity provider (if required)

            // Invalidate the token on the mobile backend
            var authUri = new Uri($"{client.MobileAppUri}/.auth/logout");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", client.CurrentUser.MobileServiceAuthenticationToken);
                await httpClient.GetAsync(authUri);
            }

            // Remove the token from the cache
            DependencyService.Get<ILoginProvider>().RemoveTokenFromSecureStore();
            Settings.AccessToken = "";
            Settings.ResetAll();

            // Remove the token from the MobileServiceClient
            await client.LogoutAsync();
        }
    }
}