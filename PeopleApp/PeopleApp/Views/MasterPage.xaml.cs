using PeopleApp.Models.ViewsRelated;
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
	public partial class MasterPage : ContentPage
	{
        public ListView ListView { get { return listView; } }
        public MasterPage ()
		{
			InitializeComponent ();

            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Home",
                IconSource = "ic_home_black_48dp.png",
                TargetType = typeof(HomePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Event Overview",
                IconSource = "ic_event_black_48dp.png",
                TargetType = typeof(EventOverviewPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "History",
                IconSource = "ic_event_black_48dp.png",
                TargetType = typeof(EventHistoryPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Profile",
                IconSource = "ic_settings_applications_black_48dp.png",
                TargetType = typeof(ProfilePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "About",
                IconSource = "ic_info_outline_black_48dp.png",
                TargetType = typeof(AboutPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }
}