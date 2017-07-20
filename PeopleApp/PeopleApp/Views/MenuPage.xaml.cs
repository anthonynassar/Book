using PeopleApp.Models.ViewsRelated;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : MasterDetailPage
    {
		public MenuPage ()
		{
			InitializeComponent ();
            this.Title = "Menu Page";
            masterPage.ListView.ItemSelected += OnItemSelected;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                //await Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType));
                Detail = (Page)Activator.CreateInstance(item.TargetType);
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}