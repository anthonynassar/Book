﻿using Microsoft.WindowsAzure.MobileServices;
using PeopleApp.Models;
using PeopleApp.Models.ViewsRelated;
using System.Threading.Tasks;

namespace PeopleApp.Abstractions
{
    public interface ICloudService
    {
        //ICloudTable<T> GetTable<T>() where T : TableData;
        Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData;
        //Task<ICloudTable<T>> AddItemAsync<T>() where T : TableData;
        

        Task<MobileServiceUser> LoginAsync(string provider);

        Task LogoutAsync();

        Task<AppServiceIdentity> GetIdentityAsync();

        Task SyncOfflineCacheAsync();

        // Simple Operations
        Task<SharingSpace> AddSharingSpace(SharingSpace sharingSpace);
        Task<Models.Object> AddObject(Models.Object @object);
        Task<SharingSpace> GetSharingSpace(string id);
    }
}