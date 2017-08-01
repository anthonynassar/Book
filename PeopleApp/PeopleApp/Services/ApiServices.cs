using Newtonsoft.Json;
using PeopleApp.Models;
using PeopleApp.Models.ViewsRelated;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PeopleApp.Services
{
    class ApiServices
    {
        public async Task<List<UserInfo>> GetUserInfoAsync(string token)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + ".auth/me");
            try
            {
                var userInfo = JsonConvert.DeserializeObject<List<UserInfo>>(json);

                return userInfo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: "+ex.Message);
                Debug.WriteLine("Error: " + ex);
                return null;
            }
        }

        public async Task PostUserAsync(User user, string token)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");
            content.Headers.Add("X-ZUMO-AUTH", token);

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/user", content);
        }

        //public  Task GetSSCountAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<SharingSpace>> GetSharingSpaceAsync(string token)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "tables/sharingspace");

            var sharingSpaces = JsonConvert.DeserializeObject<List<SharingSpace>>(json);

            return sharingSpaces;
        }

        //public async Task PostSharingSpaceAsync(Idea idea, string accessToken)
        public async Task<HttpResponseMessage> PostSharingSpaceAsync(SharingSpace sharingSpace, string token)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(sharingSpace);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");
            content.Headers.Add("X-ZUMO-AUTH", token);

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/sharingspace", content);
            return response;
        }

        public async Task PostDimensionAsync(Dimension dimension)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(dimension);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/dimension", content);
        }

        public async Task PostConstraintAsync(Constraint constraint)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(constraint);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/constraint", content);
        }

        public async Task PostEventAsync(Event @event)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(@event);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "api/event", content);
            Debug.WriteLine(response.Content);
        }

        public async Task PostObjectAsync(Models.Object obj)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/object", content);
            //Debug.WriteLine(response.Content);
        }

        public async Task PostDimDatatypeAsync(DimDatatype dimDatatype)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(dimDatatype);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "api/dimdatatype", content);
            //Debug.WriteLine(response.Content);
        }

        public async Task PostDatatypeAsync(Datatype datatype)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(datatype);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/datatype", content);

        }
    }
}
