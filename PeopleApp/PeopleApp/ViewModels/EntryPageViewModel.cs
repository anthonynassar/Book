using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Abstractions;
using PeopleApp.Helpers;
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
                Debug.WriteLine("User ID: " + user.UserId);
                Settings.IdentityProvider = "aad";
                Application.Current.MainPage = new NavigationPage(new Views.TaskList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
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
                Debug.WriteLine("User ID: " + user.UserId);
                Settings.IdentityProvider = "facebook";
                Application.Current.MainPage = new NavigationPage(new Views.TaskList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
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