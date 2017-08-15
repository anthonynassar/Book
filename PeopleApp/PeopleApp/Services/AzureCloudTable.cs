using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Abstractions;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading;
using System.Diagnostics;

namespace PeopleApp.Services
{
    public class AzureCloudTable<T> : ICloudTable<T> where T : TableData
    {
        private IMobileServiceSyncTable<T> table;

        public AzureCloudTable(MobileServiceClient client)
        {
            table = client.GetSyncTable<T>();
        }

        #region ICloudTable interface
        public async Task PullAsync()
        {
            string queryName = $"incsync_{typeof(T).Name}";
            await table.PullAsync(queryName, table.CreateQuery());
        }

        public async Task<T> CreateItemAsync(T item)
        {
            await table.InsertAsync(item);
            return item;
        }

        public async Task<T> UpsertItemAsync(T item)
        {
            return (item.Id == null) ?
                await CreateItemAsync(item) :
                await UpdateItemAsync(item);
        }

        //public async Task DeleteItemAsync(T item)
        //{
        //    await table.DeleteAsync(item);
        //}
        // shorthand version better
        public async Task DeleteItemAsync(T item) => await table.DeleteAsync(item);

        public async Task<ICollection<T>> ReadAllItemsAsync()
        {
            List<T> allItems = new List<T>();

            var pageSize = 50;
            var hasMore = true;
            while (hasMore)
            {
                var pageOfItems = await table.Skip(allItems.Count).Take(pageSize).ToListAsync();
                if (pageOfItems.Count > 0)
                {
                    allItems.AddRange(pageOfItems);
                }
                else
                {
                    hasMore = false;
                }
            }
            return allItems;
        }

        public async Task<T> ReadItemAsync(string id)
        {
            return await table.LookupAsync(id);
        }

        public async Task<ICollection<T>> ReadItemsAsync(int start, int count)
        {
            return await table.Skip(start).Take(count).ToListAsync();
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            try
            {
                await table.UpdateAsync(item);
                return item;
            }

            catch (System.Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                //throw;
                return item;
            }
        }


        public async Task PurgeAsync(string queryId, string query, bool force, CancellationToken token)
        {
            string queryName = $"incsync_{typeof(T).Name}";
            await table.PurgeAsync(queryName, query, force, token);
        }


        #endregion
    }
}