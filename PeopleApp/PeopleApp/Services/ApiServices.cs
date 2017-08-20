using Newtonsoft.Json;
using PeopleApp.Models;
using PeopleApp.Models.ViewsRelated;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PeopleApp.Services
{
    class ApiServices
    {
        public async Task<List<AppServiceIdentity>> GetUserInfoAsync(string token)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + ".auth/me");
            try
            {
                var userInfo = JsonConvert.DeserializeObject<List<AppServiceIdentity>>(json);

                return userInfo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: "+ex.Message);
                Debug.WriteLine("Error: " + ex);
                return null;
            }
        }

        public async Task<List<SharingSpace>> GetNearbyEvents(string token, string distance)
        {
            distance = "500";
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);
            //var json = JsonConvert.SerializeObject(user);
            // HttpContent content = new StringContent(json);
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //content.Headers.Add("ZUMO-API-VERSION", "2.0.0");
            //content.Headers.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "api/custom/" + distance);

            var sharingspaces = JsonConvert.DeserializeObject<List<SharingSpace>>(json);

            return sharingspaces;

        }

        public async Task<User> PostUserAsync(User user, string token)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");
            content.Headers.Add("X-ZUMO-AUTH", token);

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/user", content);

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                try
                {
                    var returnedObjects = response.Content.ReadAsStringAsync().Result;
                    var returnedUser = (User)JsonConvert.DeserializeObject(returnedObjects, typeof(User));

                    return returnedUser;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    Debug.WriteLine("Error: " + ex);
                    return null;
                }
            } else
            {
                return null;
            }
            
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

        public async Task<List<SpecialDatatype>> GetDatatypesAsync(string sharingSpaceId)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "tables/datatype/special/" + sharingSpaceId);

            var datatypes = JsonConvert.DeserializeObject<List<SpecialDatatype>>(json);

            return datatypes;
        }

        public async Task<List<Models.Object>> GetObjectsBySharingSpace(string sharingSpaceId)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "tables/object/special/" + sharingSpaceId);

            var objects = JsonConvert.DeserializeObject<List<Models.Object>>(json);

            return objects;
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

        public async Task PostAttributeAsync(Models.Attribute attribute)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(attribute);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "api/attribute", content);
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

        // Data class
        public class SpecialDatatype
        {
            public string DatatypeId { get; set; }
            public string Label { get; set; }
        }
    }
}
