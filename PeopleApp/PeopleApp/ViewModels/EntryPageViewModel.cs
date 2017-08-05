using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            LoginCommand = new Command(async () => await ExecuteLoginCommand());
            FbLoginCommand = new Command(async () => await ExecuteFbLoginCommand());
        }
        public Command LoginCommand { get; }
        public Command FbLoginCommand { get; }

        async Task ExecuteLoginCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                //await cloudService.LoginAsync(User);
                
                MobileServiceUser user = await cloudService.LoginAsync("aad");
                User currentUser = new User();
                // add user to db
                currentUser = await _apiServices.PostUserAsync(currentUser, user.MobileServiceAuthenticationToken);
                if (currentUser != null && currentUser.Id != null)
                {
                    Settings.AccessToken = user.MobileServiceAuthenticationToken;
                    Settings.IdentityProvider = "aad";
                    //Settings.UserId = "aad" + "_" + user.UserId.Split(':')[1];
                    Settings.UserId = currentUser.Id;
                    Application.Current.MainPage = new Views.MenuPage();
                }
                else
                {
                    Application.Current.MainPage = new Views.MenuPage();
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Error Full: ======/n" + ex);
                await Application.Current.MainPage.DisplayAlert("Login Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteFbLoginCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                //await cloudService.LoginAsync(User);

                MobileServiceUser user = await cloudService.LoginAsync("facebook");
                User currentUser = new User();
                // add user to db
                await _apiServices.PostUserAsync(currentUser, user.MobileServiceAuthenticationToken);
                Settings.AccessToken = user.MobileServiceAuthenticationToken;
                Settings.IdentityProvider = "facebook";
                Settings.UserId = "facebook" + "_" + user.UserId.Split(':')[1];
                Application.Current.MainPage = new Views.MenuPage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                Debug.WriteLine("Error Full: ======/n" + ex);
                await Application.Current.MainPage.DisplayAlert("Login Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}