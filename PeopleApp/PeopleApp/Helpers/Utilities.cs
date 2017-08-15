using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PeopleApp.Helpers
{
    public static class Utilities
    {
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static async Task ResolveConflictAsync<T>(MobileServiceTableOperationError error) where T : TableData
        {
            var serverItem = error.Result.ToObject<T>();
            var localItem = error.Item.ToObject<T>();

            // Note that you need to implement the public override Equals(TodoItem item)
            // method in the Model for this to work
            if (serverItem.Equals(localItem))
            {
                // Items are the same, so ignore the conflict
                await error.CancelAndDiscardItemAsync();
                return;
            }

            // Client Always Wins
            localItem.Version = serverItem.Version;
            await error.UpdateOperationAsync(JObject.FromObject(localItem));

            // Server Always Wins
            // await error.CancelAndDiscardItemAsync();
        }
    }
}
