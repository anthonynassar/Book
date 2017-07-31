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
	public partial class EntryPage : ContentPage
	{
		public EntryPage ()
		{
			InitializeComponent ();
            BindingContext = new EntryPageViewModel();
        }

        //private async void Button_ClickedAsync(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new MenuPage());
        //}
    }
}