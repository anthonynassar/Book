using System.Threading.Tasks;

namespace PeopleApp.Abstractions
{
    public interface ICloudService
    {
        ICloudTable<T> GetTable<T>() where T : TableData;

        Task LoginAsync();
        Task LogoutAsync();
    }
}