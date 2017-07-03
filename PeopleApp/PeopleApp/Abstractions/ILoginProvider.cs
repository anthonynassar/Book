using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace PeopleApp.Abstractions
{
    public interface ILoginProvider
    {
        MobileServiceUser RetrieveTokenFromSecureStore();

        void StoreTokenInSecureStore(MobileServiceUser user);

        void RemoveTokenFromSecureStore();

        //Task<MobileServiceUser> LoginAsync(MobileServiceClient client);
        Task LoginAsync(MobileServiceClient client);
    }
}