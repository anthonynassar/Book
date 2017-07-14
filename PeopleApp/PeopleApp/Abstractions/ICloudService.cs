using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace PeopleApp.Abstractions
{
    public interface ICloudService
    {
        ICloudTable<T> GetTable<T>() where T : TableData;

        Task<MobileServiceUser> LoginAsync(string provider);

        Task LogoutAsync();

        //Task<AppServiceIdentity> GetIdentityAsync();
    }
}