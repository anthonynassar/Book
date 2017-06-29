using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Abstractions;
using Xamarin.Forms;
using PeopleApp.Helpers;

namespace PeopleApp.Services
{
    public class AzureCloudService : ICloudService
    {
        MobileServiceClient client;

        public AzureCloudService()
        {
            client = new MobileServiceClient(Locations.AppServiceUrl);
            if (Locations.AlternateLoginHost != null)
                client.AlternateLoginHost = new Uri(Locations.AlternateLoginHost);
        }

        public ICloudTable<T> GetTable<T>() where T : TableData => new AzureCloudTable<T>(client);

        public Task LoginAsync()
        {
            var loginProvider = DependencyService.Get<ILoginProvider>();
            return loginProvider.LoginAsync(client);
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}