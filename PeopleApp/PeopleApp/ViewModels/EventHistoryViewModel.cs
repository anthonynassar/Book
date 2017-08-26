using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PeopleApp.ViewModels
{
    class EventHistoryViewModel : Abstractions.BaseViewModel
    {
        bool hasMoreItems = true;

        public EventHistoryViewModel()
        {
            Title = "Task List";
            //Table = await CloudService.GetTableAsync<SharingSpace>();
            //items.CollectionChanged += this.OnCollectionChanged;

            RefreshCommand = new Command(async () => await Refresh());
            CreateSharingSpaceCommand = new Command(async () => await CreateSharingSpace());
            //TagsCommand = new Command(async () => await NavigateToTags());
            LoadMoreCommand = new Command<SharingSpace>(async (SharingSpace item) => await LoadMore(item));

            //// Subscribe to events from the Task Detail Page
            //MessagingCenter.Subscribe<TaskDetailViewModel>(this, "ItemsChanged", async (sender) =>
            //{
            //    await Refresh();
            //});

            // Execute the refresh command
            RefreshCommand.Execute(null);
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ILoginProvider LoginProvider => DependencyService.Get<ILoginProvider>();
        public ICommand RefreshCommand { get; }
        public ICommand CreateSharingSpaceCommand { get; }
        public ICommand LoadMoreCommand { get; }

        ObservableRangeCollection<SharingSpace> items = new ObservableRangeCollection<SharingSpace>();
        public ObservableRangeCollection<SharingSpace> Items
        {
            get { return items; }
            set { SetProperty(ref items, value, "Items"); }
        }

        SharingSpace selectedItem;
        public SharingSpace SelectedItem
        {
            get { return selectedItem; }
            set
            {
                SetProperty(ref selectedItem, value, "SelectedItem");
                if (selectedItem != null)
                {
                    MessagingCenter.Send(this, "SelectEvent", selectedItem);
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
                //var identity = await CloudService.GetIdentityAsync();
                //if (identity != null)
                //{
                //    var name = identity.UserClaims.FirstOrDefault(c => c.Type.Equals("name")).Value;
                //    Title = $"Tasks for {name}";
                //}
                var table = await CloudService.GetTableAsync<SharingSpace>();
                var list = await table.ReadItemsAsync(0, 20);
                //var list = await table.ReadAllItemsAsync();
                Items.ReplaceRange(list);
                hasMoreItems = true; // Reset for refresh
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskList] Error loading items: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Refresh problem", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task CreateSharingSpace()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                //await Application.Current.MainPage.Navigation.PushAsync(new Views.CreateSharingSpaceAPage());
                IsBusy = true;

                var sharingSpace = await CloudService.AddSharingSpace(new SharingSpace { UserId = Settings.UserId, Descriptor = "A very special event", CreationLocation = "Anglet", CreationDate = DateTime.Now });
                Items.Add(sharingSpace);
                //SortCoffees();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskList] Error in CreateSharingSpace: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Sharing space Not Created", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task LoadMore(SharingSpace item)
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
            var table = await CloudService.GetTableAsync<SharingSpace>();
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