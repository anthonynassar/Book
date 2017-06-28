using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace PeopleApp.Abstractions
{
    public interface ILoginProvider
    {
        Task<MobileServiceUser> LoginAsync(MobileServiceClient client);
    }
}