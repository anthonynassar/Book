using PeopleApp.Models.ViewsRelated;
using PeopleApp.Services;
using PeopleApp.Helpers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : MasterDetailPage
    {
        ApiServices _apiServices = new ApiServices();
        public static NavigationPage NavPage { get; private set; }

        public MenuPage ()
		{
			InitializeComponent ();
            this.Title = "Event sharing app";
            //NavigationPage.SetHasNavigationBar(this, false);
            masterPage.ListView.ItemSelected += OnItemSelected;
            // maybe i won't use this
            //var sharingSpaceCount = await _apiServices.GetSSCountAsync();
            //Settings.SharingSpaceCount = sharingSpaceCount;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                // option 1
                //Detail = (Page)Activator.CreateInstance(item.TargetType);
                // option 2
                NavPage = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                Detail = NavPage;
                //Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

                //NavigationPage.SetHasNavigationBar(this, false);
                //Detail.BindingContext to pass data like constructor
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}