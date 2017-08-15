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
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
			InitializeComponent ();
        }

        private void LearnMore_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("http://spider.sigappfr.org/"));
        }
    }
}