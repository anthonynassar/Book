using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.Models.ViewsRelated;
using PeopleApp.ViewModels;
using PeopleApp.Views;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PeopleApp.ViewModels
{
    public class TaskListViewModel : Abstractions.BaseViewModel
    {
        bool hasMoreItems = true;

        public TaskListViewModel()
        {
            Title = "Task List";
            //Table = await CloudService.GetTableAsync<TodoItem>();
            //items.CollectionChanged += this.OnCollectionChanged;

            RefreshCommand = new Command(async () => await Refresh());
            AddNewItemCommand = new Command(async () => await AddNewItem());
            LogoutCommand = new Command(async () => await Logout());
            //TagsCommand = new Command(async () => await NavigateToTags());
            //LoadMoreCommand = new Command<TodoItem>(async (TodoItem item) => await LoadMore(item));

            // Subscribe to events from the Task Detail Page
            MessagingCenter.Subscribe<TaskDetailViewModel>(this, "ItemsChanged", async (sender) =>
            {
                await Refresh();
            });

            // Execute the refresh command
            RefreshCommand.Execute(null);
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ILoginProvider PlatformProvider => DependencyService.Get<ILoginProvider>();
        public ICloudTable<TodoItem> Table { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand AddNewItemCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public ICommand TagsCommand { get; }

        //void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    Debug.WriteLine("[TaskList] OnCollectionChanged: Items have changed");
        //}

        ObservableRangeCollection<TodoItem> items = new ObservableRangeCollection<TodoItem>();
        public ObservableRangeCollection<TodoItem> Items
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

        async Task Refresh()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await CloudService.SyncOfflineCacheAsync();
                var identity = await CloudService.GetIdentityAsync();
                if (identity != null)
                {
                    var name = identity.UserClaims.FirstOrDefault(c => c.Type.Equals("name")).Value;
                    Title = $"Tasks for {name}";
                }
                var table = await CloudService.GetTableAsync<TodoItem>();
                var list = await table.ReadItemsAsync(0, 20);
                Items.ReplaceRange(list);
                hasMoreItems = true; // Reset for refresh
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Items Not Loaded", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        async Task AddNewItem()
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
                await Application.Current.MainPage.DisplayAlert("Item Not Added", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task Logout()
        {
            if (IsBusy)
                return;
            IsBusy = true;

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
                IsBusy = false;
            }
        }
        async Task LoadMore(TodoItem item)
        {
            if (IsBusy)
            {
                Debug.WriteLine($"LoadMore: bailing because IsBusy = true");
                return;
            }

            // If we are not displaying the last one in the list, then return.
            if (!Items.Last().Id.Equals(item.Id))
            {
                Debug.WriteLine($"LoadMore: bailing because this id is not the last id in the list");
                return;
            }

            // If we don't have more items, return
            if (!hasMoreItems)
            {
                Debug.WriteLine($"LoadMore: bailing because we don't have any more items");
                return;
            }

            IsBusy = true;
            var table = await CloudService.GetTableAsync<TodoItem>();
            try
            {
                var list = await table.ReadItemsAsync(Items.Count, 20);
                if (list.Count > 0)
                {
                    Debug.WriteLine($"LoadMore: got {list.Count} more items");
                    Items.AddRange(list);
                }
                else
                {
                    Debug.WriteLine($"LoadMore: no more items: setting hasMoreItems= false");
                    hasMoreItems = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("LoadMore Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}