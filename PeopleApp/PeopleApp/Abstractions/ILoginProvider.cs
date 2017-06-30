using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace PeopleApp.Abstractions
{
    public interface ILoginProvider
    {
        Task LoginAsync(MobileServiceClient client);
    }
}