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
    public partial class EventHistoryPage : TabbedPage
    {
        ApiServices _apiServices = new ApiServices();

        public EventHistoryPage ()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<OwnerEventsViewModel, SharingSpace>(this, "SelectOwnerEvent", OnSelectedSharingSpace);
            MessagingCenter.Subscribe<ParticipantEventsViewModel, SharingSpace>(this, "SelectEvent", OnSelectedSharingSpace);
        }

        private async void OnSelectedSharingSpace(Abstractions.BaseViewModel source, SharingSpace sharingSpace)
        {
            try
            {
                var objectList = await _apiServices.GetObjectsBySharingSpace(sharingSpace.Id);
                Navigation.InsertPageBefore(new EventOverviewPage(sharingSpace, objectList), this);//Navigation.NavigationStack.OfType<EventHistoryPage>().FirstOrDefault()); // this);
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
            MessagingCenter.Unsubscribe<OwnerEventsViewModel, SharingSpace>(this, "SelectOwnerEvent");// .Subscribe<EventHistoryViewModel, SharingSpace>(this, "SelectEvent", OnSelectedSharingSpace);
            MessagingCenter.Unsubscribe<ParticipantEventsViewModel, SharingSpace>(this, "SelectEvent");
        }
    }
}