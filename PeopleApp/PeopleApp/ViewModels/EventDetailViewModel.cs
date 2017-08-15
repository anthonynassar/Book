using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PeopleApp.ViewModels
{
    public class EventDetailViewModel : Abstractions.BaseViewModel
    {
        public EventDetailViewModel(SharingSpace sharingSpace = null)
        {
            // save the id of current sharing space
            Settings.CurrentSharingSpace = sharingSpace.Id;

            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command(async () => await DeleteAsync());

            if (sharingSpace != null)
            {
                CurrentSharingSpace = sharingSpace;
                Title = "Event Description";
            }
            else
            {
                CurrentSharingSpace = new SharingSpace { Descriptor = "New Item", CreationLocation = "Anglet", CreationDate = DateTime.UtcNow };
                Title = "New Item";
            }

        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ILoginProvider LoginProvider => DependencyService.Get<ILoginProvider>();
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public Command RefreshCommand { get; }

        SharingSpace currentSharingSpace;
        public SharingSpace CurrentSharingSpace
        {
            get { return currentSharingSpace; }
            set { SetProperty(ref currentSharingSpace, value, "CurrentTask"); }
        }


        public string Descriptor
        {
            get
            {
                return CurrentSharingSpace.Descriptor;
            }
            set
            {
                var cText = CurrentSharingSpace.Descriptor;
                SetProperty(ref cText, value, "Text");
                CurrentSharingSpace.Descriptor = cText;
            }
        }


        async Task SaveAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var table = await CloudService.GetTableAsync<SharingSpace>();
                await table.UpsertItemAsync(CurrentSharingSpace);
                await CloudService.SyncOfflineCacheAsync();
                MessagingCenter.Send<EventDetailViewModel>(this, "ItemsChanged");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Save Item Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task DeleteAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                if (CurrentSharingSpace.Id != null)
                {
                    var table = await CloudService.GetTableAsync<SharingSpace>();
                    await table.DeleteItemAsync(CurrentSharingSpace);
                    await CloudService.SyncOfflineCacheAsync();
                    MessagingCenter.Send<EventDetailViewModel>(this, "ItemsChanged");
                }
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Delete Item Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}