using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.ViewModels;
using PeopleApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PeopleApp.ViewModels
{
    public class TaskListViewModel : BaseViewModel
    {
        ICloudService cloudService;

        public TaskListViewModel()
        {
            Debug.WriteLine("In TaskListViewMOdel");
            cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            Table = cloudService.GetTable<TodoItem>();


            Title = "Task List";
            items.CollectionChanged += this.OnCollectionChanged;

            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
            AddNewItemCommand = new Command(async () => await ExecuteAddNewItemCommand());
            LogoutCommand = new Command(async () => await ExecuteLogoutCommand());

            // Execute the refresh command
            RefreshCommand.Execute(null);
        }

        public ICloudTable<TodoItem> Table { get; set; }
        public Command RefreshCommand { get; }
        public Command AddNewItemCommand { get; }
        public Command LogoutCommand { get; }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("[TaskList] OnCollectionChanged: Items have changed");
        }

        ObservableCollection<TodoItem> items = new ObservableCollection<TodoItem>();
        public ObservableCollection<TodoItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value, "Items"); }
        }

        TodoItem selectedItem;
        public TodoItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                SetProperty(ref selectedItem, value, "SelectedItem");
                if (selectedItem != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new Views.TaskDetail(selectedItem));
                    SelectedItem = null;
                }
            }
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var table = App.CloudService.GetTable<TodoItem>();
                var list = await table.ReadAllItemsAsync();
                Items.Clear();
                foreach (var item in list)
                    Items.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskList] Error loading items: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteAddNewItemCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.TaskDetail());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskList] Error in AddNewItem: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLogoutCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                await cloudService.LogoutAsync();
                Application.Current.MainPage = new NavigationPage(new Views.EntryPage());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Logout Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task RefreshList()
        {
            await ExecuteRefreshCommand();
            MessagingCenter.Subscribe<TaskDetailViewModel>(this, "ItemsChanged", async (sender) =>
            {
                await ExecuteRefreshCommand();
            });
        }
    }
}