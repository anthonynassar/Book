using Newtonsoft.Json;
using PeopleApp.Models;
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
        public async Task PostUserAsync(User user, string token)
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");
            content.Headers.Add("X-ZUMO-AUTH", token);

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/user", content);
        }

        //public async Task PostSharingSpaceAsync(Idea idea, string accessToken)
        public async Task<HttpResponseMessage> PostSharingSpaceAsync(SharingSpace sharingSpace)
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(sharingSpace);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/sharingspace", content);
            return response;
        }

        public async Task PostDimensionAsync(Dimension dimension)
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(dimension);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/dimension", content);
        }

        public async Task PostConstraintAsync(Constraint constraint)
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(constraint);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/constraint", content);
        }

        public async Task PostEventAsync(Event @event)
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(@event);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "api/event", content);
            Debug.WriteLine(response.Content);
        }
    }
}
