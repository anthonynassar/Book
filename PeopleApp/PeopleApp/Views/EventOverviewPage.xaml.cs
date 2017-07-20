using PeopleApp.Models.ViewsRelated;
using PeopleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventOverviewPage : ContentPage
	{
		public EventOverviewPage ()
		{
			InitializeComponent ();
		}

        private async void PhotoItemTappedAsync(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var vm = new EventOverviewViewModel();
            var photo = e.Item as Photo;

            await Navigation.PushAsync(new FullScreenImagePage(photo.ImageUrl, "This is a description text", photo.Index, vm.Count));
        }
    }
}