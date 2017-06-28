using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PeopleApp.ViewModels
{
    public class TaskDetailViewModel : BaseViewModel
    {
        ICloudTable<TodoItem> table = App.CloudService.GetTable<TodoItem>();

        public TaskDetailViewModel(TodoItem item = null)
        {

            ICloudService cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            Table = cloudService.GetTable<TodoItem>();

            if (item != null)
            {
                Item = item;
                Title = item.Text;
            }
            else
            {
                Item = new TodoItem { Text = "New Item", Complete = false };
                Title = "New Item";
            }

            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            DeleteCommand = new Command(async () => await ExecuteDeleteCommand());
        }

        public TodoItem Item { get; set; }
        public ICloudTable<TodoItem> Table { get; set; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        async Task ExecuteSaveCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                if (Item.Id == null)
                {
                    await table.CreateItemAsync(Item);
                }
                else
                {
                    await table.UpdateItemAsync(Item);
                }
                MessagingCenter.Send<TaskDetailViewModel>(this, "ItemsChanged");
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
                if (Item.Id != null)
                {
                    await table.DeleteItemAsync(Item);
                }
                MessagingCenter.Send<TaskDetailViewModel>(this, "ItemsChanged");
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