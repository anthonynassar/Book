using PeopleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeopleApp.Models;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventList : ContentPage
	{
        private List<SharingSpace> sharingSpaces;


        public EventList(List<SharingSpace> sharingSpaces = null)
        {
            this.sharingSpaces = sharingSpaces;
            InitializeComponent();
            // BindingContext = new EventListViewModel(sharingSpaces);
            BindingContext = new EventListViewModel();
        }
    }
}
