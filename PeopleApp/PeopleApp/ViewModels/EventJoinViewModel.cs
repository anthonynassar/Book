using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using PeopleApp.Models.ViewsRelated;
using PeopleApp.Services;
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
    class EventJoinViewModel : Abstractions.BaseViewModel
    {
        ApiServices _apiServices = new ApiServices();
        private SharingSpaceView _oldItem;

        public EventJoinViewModel(List<SharingSpace> sharingSpaces)
        {
            Title = "Task List";
            List<SharingSpaceView> ssv = new List<SharingSpaceView>();
            foreach (var item in sharingSpaces)
            {
                ssv.Add(new SharingSpaceView(item, false));
            }
            Items.ReplaceRange(ssv);
            //Table = await CloudService.GetTableAsync<SharingSpace>();
            //items.CollectionChanged += this.OnCollectionChanged;

            JoinCommand = new Command(async () => await Join());
            //RefreshCommand = new Command(async () => await Refresh());
            //LoadMoreCommand = new Command<SharingSpace>(async (SharingSpace item) => await LoadMore(item));

            // Subscribe to events from the Task Detail Page
            //MessagingCenter.Subscribe<TaskDetailViewModel>(this, "ItemsChanged", async (sender) =>
            //{
            //    await Refresh();
            //});

            // Execute the refresh command
            //RefreshCommand.Execute(null);
        }

        public SharingSpace HideOrShowProduct(SharingSpaceView item)
        {
            if (_oldItem == item)
            {
                // click twice on the same item will hide it
                item.IsVisible = !item.IsVisible;
                UpdateItems(item);
            }
            else
            {
                if(_oldItem != null)
                {
                    // hide previous selected item
                    _oldItem.IsVisible = false;
                    UpdateItems(item);
                }
                // show selected item
                item.IsVisible = true;
                UpdateItems(item);
            }
            _oldItem = item;

            // save sharing space id
            if (item.IsVisible)
                return new SharingSpace { Id = item.Id, CreatedAt = item.CreatedAt, CreationDate = item.CreationDate, CreationLocation = item.CreationLocation, UserId = item.UserId, Descriptor = item.Descriptor, UpdatedAt = item.UpdatedAt, Version = item.Version };
            else
                return null;
        }

        private void UpdateItems(SharingSpaceView item)
        {
            var index = Items.IndexOf(item);
            Items.Remove(item);
            Items.Insert(index, item);
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ILoginProvider PlatformProvider => DependencyService.Get<ILoginProvider>();
        public ICloudTable<SharingSpace> Table { get; set; }
        public ICommand JoinCommand { get; }
        //public ICommand RefreshCommand { get; }
        //public ICommand LoadMoreCommand { get; }

        //void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    Debug.WriteLine("[TaskList] OnCollectionChanged: Items have changed");
        //}

        ObservableRangeCollection<SharingSpace> items = new ObservableRangeCollection<SharingSpace>();
        public ObservableRangeCollection<SharingSpace> Items
        {
            get { return items; }
            set { SetProperty(ref items, value, "Items"); }
        }

        //SharingSpace selectedItem;
        //public SharingSpace SelectedItem
        //{
        //    get { return selectedItem; }
        //    set
        //    {
        //        SetProperty(ref selectedItem, value, "SelectedItem");
        //        if (selectedItem != null)
        //        {
        //            //Application.Current.MainPage.Navigation.PushAsync(new Views.EventDetail(selectedItem));
        //            SelectedItem = null;
        //        }
        //    }
        //}

        async Task Join()
        {
            // moved
        }

        //async Task Refresh()
        //{
        //    if (IsBusy)
        //        return;
        //    IsBusy = true;

        //    try
        //    {
        //        await CloudService.SyncOfflineCacheAsync();
        //        var identity = await CloudService.GetIdentityAsync();
        //        if (identity != null)
        //        {
        //            var name = identity.UserClaims.FirstOrDefault(c => c.Type.Equals("name")).Value;
        //            Title = $"Tasks for {name}";
        //        }
        //        var table = await CloudService.GetTableAsync<SharingSpace>();
        //        var list = await table.ReadItemsAsync(0, 20);
        //        Items.ReplaceRange(list);
        //        hasMoreItems = true; // Reset for refresh
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Items Not Loaded", ex.Message, "OK");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        //async Task LoadMore(SharingSpace item)
        //{
        //    if (IsBusy)
        //    {
        //        Debug.WriteLine($"LoadMore: bailing because IsBusy = true");
        //        return;
        //    }

        //    // If we are not displaying the last one in the list, then return.
        //    if (!Items.Last().Id.Equals(item.Id))
        //    {
        //        Debug.WriteLine($"LoadMore: bailing because this id is not the last id in the list");
        //        return;
        //    }

        //    // If we don't have more items, return
        //    if (!hasMoreItems)
        //    {
        //        Debug.WriteLine($"LoadMore: bailing because we don't have any more items");
        //        return;
        //    }

        //    IsBusy = true;
        //    var table = await CloudService.GetTableAsync<SharingSpace>();
        //    try
        //    {
        //        var list = await table.ReadItemsAsync(Items.Count, 20);
        //        if (list.Count > 0)
        //        {
        //            Debug.WriteLine($"LoadMore: got {list.Count} more items");
        //            Items.AddRange(list);
        //        }
        //        else
        //        {
        //            Debug.WriteLine($"LoadMore: no more items: setting hasMoreItems= false");
        //            hasMoreItems = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("LoadMore Failed", ex.Message, "OK");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
    }
}