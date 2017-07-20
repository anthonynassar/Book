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
        public EventDetailViewModel(SharingSpace item = null)
        {
            ICloudService cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            Table = cloudService.GetTable<SharingSpace>();

            if (item != null)
            {
                SharingSpace = item;
                Title = item.Descriptor;
            }
            else
            {
                SharingSpace = new SharingSpace { Descriptor = "New Item", CreationLocation = "Anglet", CreationDate = DateTime.UtcNow };
                Title = "New Item";
            }

            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            DeleteCommand = new Command(async () => await ExecuteDeleteCommand());
        }

        public SharingSpace SharingSpace { get; set; }
        public ICloudTable<SharingSpace> Table { get; set; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        async Task ExecuteSaveCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                if (SharingSpace.Id == null)
                {
                    await Table.CreateItemAsync(SharingSpace);
                }
                else
                {
                    await Table.UpdateItemAsync(SharingSpace);
                }
                MessagingCenter.Send<EventDetailViewModel>(this, "ItemsChanged");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskDetail] Save error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Save Item Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteDeleteCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                if (SharingSpace.Id != null)
                {
                    await Table.DeleteItemAsync(SharingSpace);
                }
                MessagingCenter.Send<EventDetailViewModel>(this, "ItemsChanged");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskDetail] Save error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Delete Item Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
