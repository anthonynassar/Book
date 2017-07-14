using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeopleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateSharingSpaceAPage : ContentPage
	{
		public CreateSharingSpaceAPage ()
		{
			InitializeComponent ();
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

        

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateSharingSpaceBPage());
        }
    }
}