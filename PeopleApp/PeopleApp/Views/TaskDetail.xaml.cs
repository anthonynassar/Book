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
	public partial class TaskDetail : ContentPage
	{
		public TaskDetail ()
		{
			InitializeComponent ();
            BindingContext = new TaskDetailViewModel(item);
        }
	}
}