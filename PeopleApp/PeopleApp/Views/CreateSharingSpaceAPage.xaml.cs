using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeopleApp.Models;
using PeopleApp.Helpers;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateSharingSpaceAPage : ContentPage
	{
		public CreateSharingSpaceAPage ()
		{
			InitializeComponent ();
            //NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            try
            {

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                if (position != null)
                {
                    LongitudeLabel.Detail = position.Longitude.ToString();
                    LatitudeLabel.Detail = position.Latitude.ToString();
                }
                else
                {
                    LongitudeLabel.Detail = "No data!";
                    LatitudeLabel.Detail = "No data!";
                }

                var addresses = await locator.GetAddressesForPositionAsync(position);
                var address = addresses.FirstOrDefault();

                if (address == null)
                {
                    Debug.WriteLine("No address found for position.");
                    AddressLabel.Detail = "No address found for position.";
                }
                else
                {
                    Debug.WriteLine("Address: {0} {1} {2}", address.Thoroughfare, address.Locality, address.CountryName);
                    AddressLabel.Detail = $"Address: {address.Thoroughfare} {address.Locality} {address.CountryName}";
                }
            }
            catch (TaskCanceledException e)
            {
                // Exception of type: System.Threading.Tasks.TaskCanceledException
                Debug.WriteLine("Error " + e.Message);
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + e);
                LongitudeLabel.Detail = "No data!";
                LatitudeLabel.Detail = "No data!";
                AddressLabel.Detail = "No data!";
            }
            catch (Exception e)
            {
                AddressLabel.Detail = "Unable to get address at this moment!";
                Debug.WriteLine("Unable to get address: " + e);
            }
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            // save sharing space and constraints
            SharingSpace sharingSpace = new SharingSpace
            {
                CreationDate = DateTime.Now,
                CreationLocation = LatitudeLabel.Detail + ", " + LongitudeLabel.Detail,
                Descriptor = eventDescriptor.Text,
                Id = Utilities.NewGuid(),
                UserId = Settings.UserId
            };

            DateTime startDate = dateStart.Date + timeStart.Time;
            DateTime endDate = dateEnd.Date + timeEnd.Time;

            Models.Constraint timeConstraint1 = new Models.Constraint { Operator = "begin", Value = startDate.ToString() };
            Models.Constraint timeConstraint2 = new Models.Constraint { Operator = "end", Value = endDate.ToString() };
            Models.Constraint locationConstraint1 = new Models.Constraint { Operator = "range", Value = mySlider.Value.ToString() };
            Models.Constraint locationConstraint2 = new Models.Constraint { Operator = "latitude", Value = LatitudeLabel.Detail };
            Models.Constraint locationConstraint3 = new Models.Constraint { Operator = "longitude", Value = LongitudeLabel.Detail };
            var timeConstraintList = new List<Models.Constraint> { timeConstraint1, timeConstraint2 };
            var locationConstraintList = new List<Models.Constraint> { locationConstraint1, locationConstraint2, locationConstraint3 };
            List<DimensionView> dimensions = new List<DimensionView>
            {
                new DimensionView { Label = "Time", Interval = true, ConstraintList = timeConstraintList},
                new DimensionView { Label = "Location", Interval = true, ConstraintList = locationConstraintList}
            };

            await Navigation.PushAsync(new CreateSharingSpaceBPage(sharingSpace, dimensions));
        }
    }
}