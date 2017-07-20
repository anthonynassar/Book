using MvvmHelpers;
using PeopleApp.Abstractions;
using PeopleApp.Helpers;
using PeopleApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PeopleApp.ViewModels
{
    class EventListViewModel : Abstractions.BaseViewModel
    {
        ICloudService cloudService;

        public EventListViewModel()
        {
            Debug.WriteLine("In TaskListViewMOdel");
            cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            Table = cloudService.GetTable<SharingSpace>();


            Title = "Event List";
            items.CollectionChanged += this.OnCollectionChanged;

            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
            LogoutCommand = new Command(async () => await ExecuteLogoutCommand());
            CreateSharingSpaceCommand = new Command(async () => await ExecuteCreateSharingSpaceCommand());

            // Execute the refresh command
            RefreshCommand.Execute(null);
        }

        public ICloudTable<SharingSpace> Table { get; set; }
        public Command RefreshCommand { get; }
        public Command LogoutCommand { get; }
        public Command CreateSharingSpaceCommand { get; }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("[TaskList] OnCollectionChanged: Items have changed");
        }

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
                    Application.Current.MainPage.Navigation.PushAsync(new Views.EventDetail(selectedItem));
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
                var list = await Table.ReadAllItemsAsync();
                Items.ReplaceRange(list);
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

        async Task ExecuteCreateSharingSpaceCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.CreateSharingSpaceAPage());
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
