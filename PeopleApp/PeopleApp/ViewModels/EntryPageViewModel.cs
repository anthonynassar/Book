using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PeopleApp.ViewModels
{
    public class EntryPageViewModel : BaseViewModel
    {
        ApiServices _apiServices = new ApiServices();

        public EntryPageViewModel()
        {
            Title = "Task List";

            AppService = Locations.AppServiceUrl;
            LoginCommand = new Command(async () => await Login());
            FbLoginCommand = new Command(async () => await FbLogin());
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ILoginProvider LoginProvider => DependencyService.Get<ILoginProvider>();
        public Command LoginCommand { get; }
        public Command FbLoginCommand { get; }
        public string AppService { get; set; }

        async Task Login()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                MobileServiceUser user = await CloudService.LoginAsync("aad");
                var table = await CloudService.GetTableAsync<User>();
                string userId = "aad|" + user.UserId.Replace(':','|');
                User retrievedUser = await table.ReadItemAsync(userId);
                if (retrievedUser == null)
                {
                    await table.CreateItemAsync(new User { Id = userId, CultureInfo = CultureInfo.CurrentUICulture.ToString() });
                    await CloudService.SyncOfflineCacheAsync();
                    User tempUser = await table.ReadItemAsync(userId);
                    //User tempUser = await _apiServices.PostUserAsync(new User(), user.MobileServiceAuthenticationToken);

                    if (tempUser == null)
                        throw new NullReferenceException("Problem adding/signing in user");
                    else
                        retrievedUser = tempUser;
                }

                Settings.AccessToken = user.MobileServiceAuthenticationToken;
                Settings.IdentityProvider = "aad";
                Settings.UserId = retrievedUser.Id;
                Settings.Username = retrievedUser.Username;

                Application.Current.MainPage = new Views.MenuPage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Full error: " + ex);
                await Application.Current.MainPage.DisplayAlert("Login Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task FbLogin()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                MobileServiceUser user = await CloudService.LoginAsync("facebook");
                var table = await CloudService.GetTableAsync<User>();
                string userId = "facebook|" + user.UserId.Replace(':', '|');
                User retrievedUser = await table.ReadItemAsync(userId);
                if (retrievedUser == null)
                {
                    await table.CreateItemAsync(new User { Id = userId, CultureInfo = CultureInfo.CurrentUICulture.ToString() });
                    await CloudService.SyncOfflineCacheAsync();
                    User tempUser = await table.ReadItemAsync(userId);
                    //User tempUser = await _apiServices.PostUserAsync(new User(), user.MobileServiceAuthenticationToken);

                    if (tempUser == null)
                        throw new NullReferenceException("Problem adding/signing in user");
                    else
                        retrievedUser = tempUser;
                }
                Settings.AccessToken = user.MobileServiceAuthenticationToken;
                Settings.IdentityProvider = "facebook";
                Settings.UserId = retrievedUser.Id;
                Settings.Username = retrievedUser.Username;

                Application.Current.MainPage = new Views.MenuPage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Full error: " + ex);
                await Application.Current.MainPage.DisplayAlert("Facebook Login Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}