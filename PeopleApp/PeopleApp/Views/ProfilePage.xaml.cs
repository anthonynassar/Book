using PeopleApp.Models.ViewsRelated;
using PeopleApp.Services;
using PeopleApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeopleApp.ViewModels;
using PeopleApp.Models;
using PeopleApp.Abstractions;
using System.Globalization;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        ApiServices _apiServices = new ApiServices();
        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        private bool DataChanged = false;
        public User User { get; set; }

        public ProfilePage(User user)
        {
            if (user == null)
                throw new ArgumentNullException();

            InitializeComponent();
            birthdate.MaximumDate = DateTime.Now.AddYears(-13);
            this.User = user;
            //firstname.Text = userInfo.UserClaims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
            username.Text = user.Username;
            firstName.Text = user.GivenName;
            lastName.Text = user.Surname;
            gender.Text = user.Gender;
            email.Detail = user.Email;
            city.Text = user.City;
            country.Text = user.Country;
            //if(false)
            if(user.Birthdate.Year.Equals(1) || user.Birthdate.Year.Equals(1900))
                birthdate.IsEnabled = true;
            else
            {
                birthdate.Date = user.Birthdate;
                birthdate.IsEnabled = false;
            }

            birthdate.PropertyChanged += OnValueChanged;
            firstName.PropertyChanged += OnValueChanged;
            country.PropertyChanged += OnValueChanged;
            city.PropertyChanged += OnValueChanged;
            gender.PropertyChanged += OnValueChanged;
            lastName.PropertyChanged += OnValueChanged;
        }

        public override string ToString()
        {
            return this.Title;
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            if (activityStack.IsVisible)
                return;

            messageLabel.Text = "Terminating your session! Please wait!";
            activityStack.IsVisible = true;
            activityIndicator.IsRunning = true;
           

            try
            {
                await CloudService.LogoutAsync();
                Application.Current.MainPage = new NavigationPage(new Views.EntryPage());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Logout Failed", ex.Message, "OK");
            }
            finally
            {
                activityStack.IsVisible = false;
                activityIndicator.IsRunning = false;
            }
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            if (activityStack.IsVisible)
                return;

            activityStack.IsVisible = true;
            activityIndicator.IsRunning = true;
            messageLabel.Text = "Updating your profile!";
            try
            {
                
                if (DataChanged)
                {
                    var table = await CloudService.GetTableAsync<User>();

                    User updatedUser = new User
                    {
                        Id = Settings.UserId,
                        GivenName = firstName.Text,
                        Surname = lastName.Text,
                        Gender = gender.Text,
                        CultureInfo = CultureInfo.CurrentUICulture.ToString(),
                        Birthdate = birthdate.Date,// DateTime.Parse(birthdate.Text, CultureInfo.CurrentUICulture),
                        City = city.Text,
                        Country = country.Text,
                        // unchanged properties
                        Address = User.Address,
                        Email = User.Email,
                        Privilege = User.Privilege,
                        Profession = User.Profession,
                        Username = User.Username
                    };

                    await table.UpdateItemAsync(updatedUser);
                    //User = updatedUser;
                    await CloudService.SyncOfflineCacheAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Update aborted", "No changes have been found", "OK");
                }
            }
            catch (MobileServicePushFailedException e1 )
            {
                if (e1.PushResult != null)
                {
                    foreach (var error in e1.PushResult.Errors)
                    {
                        await ResolveConflictAsync<User>(error);
                    }
                    // Sync the operation table after changes have been made
                    await CloudService.SyncOfflineCacheAsync();
                }
            }
            catch(Exception e2)
            {
                await Application.Current.MainPage.DisplayAlert("Update failed", e2.Message, "OK");
            }
            finally
            {
                activityStack.IsVisible = false;
                activityIndicator.IsRunning = false;
                DataChanged = false;
            }
        }

        private async Task ResolveConflictAsync<T>(MobileServiceTableOperationError error) where T:TableData
        {
            var serverItem = error.Result.ToObject<T>();
            var localItem = error.Item.ToObject<T>();

            // TEMP: Solve internal server error
            if (serverItem == null)
                return;

            // Note that you need to implement the public override Equals(TodoItem item)
            // method in the Model for this to work
            if (serverItem.Equals(localItem))
            {
                // Items are the same, so ignore the conflict
                await error.CancelAndDiscardItemAsync();
                return;
            }

            // Client Always Wins
            localItem.Version = serverItem.Version;
            await error.UpdateOperationAsync(JObject.FromObject(localItem));

            // Server Always Wins
            // await error.CancelAndDiscardItemAsync();
        }

        private void OnValueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (DataChanged)
                return;

            if (firstName.Text != User.GivenName ||
                lastName.Text != User.Surname ||
                gender.Text != User.Gender ||
                birthdate.Date.Year != User.Birthdate.Year ||
                city.Text != User.City ||
                country.Text != User.Country)
            {
                DataChanged = true;
            }

            return;
        }
    }
}
