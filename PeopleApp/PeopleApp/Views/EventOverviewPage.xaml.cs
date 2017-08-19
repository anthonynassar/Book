using PeopleApp.Models.ViewsRelated;
using PeopleApp.ViewModels;
using PeopleApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeopleApp.Models;
using System.IO;
using System.Collections.ObjectModel;
using PeopleApp.Services;
using System.Globalization;

namespace PeopleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventOverviewPage : ContentPage
    {
        EventOverviewViewModel vm;
        ApiServices _apiServices = new ApiServices();
        SharingSpace sharingSpace;

        public EventOverviewPage(SharingSpace sharingSpace, List<Models.Object> objectList)
        {
            if (sharingSpace == null)
                throw new ArgumentNullException();

            BindingContext = vm = new EventOverviewViewModel(sharingSpace, objectList);
            InitializeComponent();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            eventName.Text = textInfo.ToTitleCase(sharingSpace.Descriptor);

            Settings.CurrentSharingSpace = sharingSpace.Id;
            this.sharingSpace = sharingSpace;
        }

        //public EventOverviewPage(SharingSpace sharingSpace)
        //{
        //    if (sharingSpace == null)
        //        throw new ArgumentNullException();

        //    //BindingContext = vm = new EventOverviewViewModel(sharingSpace);
        //    InitializeComponent();
        //    eventName.Text = sharingSpace.Descriptor;
        //    Settings.CurrentSharingSpace = sharingSpace.Id;
        //    this.sharingSpace = sharingSpace;
        //}

        private async void PhotoItemTappedAsync(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var photo = e.Item as Photo;

            await Navigation.PushAsync(new FullScreenImagePage(photo.ImageUrl, "This is a description text", photo.Index, vm.Count));
        }
    }
}