using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Abstractions;

namespace PeopleApp.Services
{
    public class AzureCloudService : ICloudService
    {
        MobileServiceClient client;

        public AzureCloudService()
        {
            client = new MobileServiceClient("https://peopleapp3.azurewebsites.net/");
        }

        public ICloudTable<T> GetTable<T>() where T : TableData
        {
            return new AzureCloudTable<T>(client);
        }
    }
}