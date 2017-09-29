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
	public partial class ParticipantEventsPage : ContentPage
	{
        ApiServices _apiServices = new ApiServices();
        ParticipantEventsViewModel vm;

        public ParticipantEventsPage()
        {
            InitializeComponent();
            BindingContext = vm = new ParticipantEventsViewModel();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            listView.IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (vm.Items.Count <= 0)
                vm.RefreshCommand.Execute(null);
        }
    }
}