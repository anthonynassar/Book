using PeopleApp.Models;
using PeopleApp.Services;
using PeopleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeopleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventHistoryPage : ContentPage
    {
        ApiServices _apiServices = new ApiServices();

        public EventHistoryPage()
        {
            InitializeComponent();
            BindingContext = new EventHistoryViewModel();
            // problem called many times
            //MessagingCenter.Unsubscribe<EventHistoryPage, SharingSpace>(this, "SelectEvent");
            MessagingCenter.Subscribe<EventHistoryViewModel, SharingSpace>(this, "SelectEvent", OnSelectedSharingSpace);
        }


        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            listView.IsEnabled = false;
        }

        private async void OnSelectedSharingSpace(EventHistoryViewModel source, SharingSpace sharingSpace)
        {
            try
            {
                var objectList = await _apiServices.GetObjectsBySharingSpace(sharingSpace.Id);
                Navigation.InsertPageBefore(new EventOverviewPage(sharingSpace, objectList), this);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :" + ex.Message);
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<EventHistoryViewModel, SharingSpace>(this, "SelectEvent");// .Subscribe<EventHistoryViewModel, SharingSpace>(this, "SelectEvent", OnSelectedSharingSpace);
        }
    }
}