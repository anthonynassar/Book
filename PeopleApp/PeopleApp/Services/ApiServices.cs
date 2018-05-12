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
    /// <summary>
    /// Class that groups functions to send requests to the remote database.
    /// </summary>
    class ApiServices
    {
        // Functionality implemented through another logic/code.
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

        /// <summary>
        /// An HTTP request that triggers the stored procedure in the cloud to detect the nearby events.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public async Task<List<SharingSpace>> GetNearbyEvents(string token, string distance)
        {
            distance = "500";
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);
            
            try
            {
                var response = await client.PostAsync(Constants.BaseApiAddress + "api/custom/" + distance, null);
                //GetStringAsync(Constants.BaseApiAddress + "api/custom/" + distance);
                //var sharingspaces = JsonConvert.DeserializeObject<List<SharingSpace>>(json);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    try
                    {
                        var returnedObjects = response.Content.ReadAsStringAsync().Result;
                        var returnedSharingSpaces = JsonConvert.DeserializeObject<List<SharingSpace>>(returnedObjects);

                        return returnedSharingSpaces;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error: " + ex.Message);
                        Debug.WriteLine("Error: " + ex);
                        return null;
                    }
                }
                else
                {
                    return null;
                }

                //return sharingspaces;
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                return null;
            }
        }

        /// <summary>
        /// Get a sharing space object that corresponds to the ID provided as an input.
        /// </summary>
        /// <param name="sharingSpaceId">The ID of the required sharing space</param>
        /// <param name="token">Authentication token sent with every request</param>
        /// <returns>The required sharing space object</returns>
        public async Task<SharingSpace> GetSharingSpaceById(string sharingSpaceId, string token)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "tables/SharingSpace/extra/" + sharingSpaceId);

            var sharingSpaces = JsonConvert.DeserializeObject<SharingSpace>(json);

            return sharingSpaces;
        }

        /// <summary>
        /// Verify if the user is a participant in a specific sharing space.
        /// </summary>
        /// <param name="sharingSpaceId">The ID of the required sharing space</param>
        /// <param name="token">Authentication token sent with every request</param>
        /// <returns>If the user is participant, the ID of the sharing space will be returned,
        /// otherwise the output is an empty string.</returns>
        public async Task<string> VerifyUserParticipation(string sharingSpaceId, string token)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "api/custom/ss/" + sharingSpaceId);

            var response = JsonConvert.DeserializeObject<List<string>>(json);

            if (response.Count != 0)
                return response[0];
            else
                return null;
        }

        /// <summary>
        /// Verify if user is an owner of a specific sharing space.
        /// </summary>
        /// <param name="sharingSpaceId">The ID of the required sharing space</param>
        /// <param name="token">Authentication token sent with every request</param>
        /// <returns>If the user is owner, the ID of the sharing space will be returned,
        /// otherwise the output is an empty string.</returns>
        public async Task<string> VerifyOnwershipOfEvent(string sharingSpaceId, string token)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "api/custom/user/" + sharingSpaceId);

            var response = JsonConvert.DeserializeObject<List<string>>(json);

            if(response.Count != 0)
                return response[0];
            else
                return null;
        }

        // Functionality implemented through another logic/code.
        public async Task<string> GetUserBySharingSpace(string sharingSpaceId, string token)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "api/custom/ss2/" + sharingSpaceId);

            var response = JsonConvert.DeserializeObject<string>(json);

            return response;
        }

        // Functionality implemented through another logic/code.
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

        // Functionality implemented through another logic/code.
        public async Task<List<SharingSpace>> GetSharingSpaceAsync(string token)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "tables/sharingspace");

            var sharingSpaces = JsonConvert.DeserializeObject<List<SharingSpace>>(json);

            return sharingSpaces;
        }

        /// <summary>
        /// Retrieve a specific dimension's ID related to a specific sharing space.
        /// </summary>
        /// <remarks>A dimension could be of different type: Time, Geo, Social ...</remarks>
        /// <param name="sharingSpaceId">The ID of the required sharing space</param>
        /// <param name="topic">The label of the dimension</param>
        /// <param name="token">Authentication token sent with every request</param>
        /// <returns>The required sharing space object</returns>
        public async Task<string> GetDimensionId(string sharingSpaceId, string topic, string token)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "api/custom/dimension?sharingSpaceId=" + sharingSpaceId + "&topic=" + topic);

            var response = JsonConvert.DeserializeObject<List<string>>(json);

            if (response.Count != 0)
                return response[0];
            else
                return null;
        }

        /// <summary>
        /// Add a sharing space object to the remote database.
        /// </summary>
        /// <param name="sharingSpace">The sharing space to be added</param>
        /// <param name="token"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add a dimension object to the remote database.
        /// </summary>
        /// <param name="dimension">The dimension to be added</param>
        /// <returns></returns>
        public async Task PostDimensionAsync(Dimension dimension)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(dimension);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/dimension", content);
        }

        /// <summary>
        /// Add a constraint object to the remote database.
        /// </summary>
        /// <param name="constraint">The constraint to be added</param>
        /// <returns></returns>
        public async Task PostConstraintAsync(Constraint constraint)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(constraint);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/constraint", content);
        }

        /// <summary>
        /// Add an event object to the remote database.
        /// </summary>
        /// <param name="event">The event to be added</param>
        /// <returns></returns>
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

        /// <summary>
        /// Add an attribute object to the remote database.
        /// </summary>
        /// <param name="attribute">The attribute to be added</param>
        /// <returns></returns>
        public async Task PostAttributeAsync(Models.Attribute attribute)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(attribute);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "api/attribute", content);
        }

        /// <summary>
        /// Add a datatype object to the remote database.
        /// </summary>
        /// <param name="datatype">The datatype to be added</param>
        /// <returns></returns>
        public async Task PostDatatypeAsync(Datatype datatype)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(datatype);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("ZUMO-API-VERSION", "2.0.0");

            var response = await client.PostAsync(Constants.BaseApiAddress + "tables/datatype", content);

        }

        /// <summary>
        /// Add a DimDatatype object to the remote database.
        /// </summary>
        /// <param name="dimDatatype"> The dimdatatype to be added</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get each datatype for every dimension related to a specific sharing space.
        /// </summary>
        /// <param name="sharingSpaceId"></param>
        /// <returns></returns>
        public async Task<List<SpecialDatatype>> GetDatatypesAsync(string sharingSpaceId)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "tables/datatype/special/" + sharingSpaceId);

            var datatypes = JsonConvert.DeserializeObject<List<SpecialDatatype>>(json);

            return datatypes;
        }

        /// <summary>
        /// Get all objects related to a sharing space.
        /// </summary>
        /// <param name="sharingSpaceId"></param>
        /// <returns></returns>
        public async Task<List<Models.Object>> GetObjectsBySharingSpace(string sharingSpaceId)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            var json = await client.GetStringAsync(Constants.BaseApiAddress + "tables/object/special/" + sharingSpaceId);

            var objects = JsonConvert.DeserializeObject<List<Models.Object>>(json);

            return objects;
        }

        // Functionality implemented through another logic/code.
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

        /// <summary>
        /// A simple class to parse incoming results (datatypes).
        /// </summary>
        public class SpecialDatatype
        {
            public string DatatypeId { get; set; }
            public string Label { get; set; }
        }
    }
}
