using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Backend.Models;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using Backend.Helpers;
using Backend.DataObjects;
using System.Linq;
using System.Net.Http;
using System;

namespace Backend.Controllers
{
    [MobileAppController]
    //[Authorize]
    public class CustomController : ApiController
    {
        MobileServiceContext context;

        public CustomController() : base()
        {
            context = new MobileServiceContext();
        }

        // GET api/Custom
        public string Get()
        {
            return "Hello from custom controller!";
        }

        // POST api/Custom/{distance} or without distance
        [HttpPost]
        [Route("api/custom/{distance}")]
        public async Task<List<SharingSpace>> PostAsync(string distance)
        {
            try
            {
                // Get the database from the context.
                var database = context.Database;

                //PARAMETERS
                //var myParam = new System.Data.SqlClient.SqlParameter("@dist", 1);

                // Create a SQL statement that sets all uncompleted items
                // to complete and execute the statement asynchronously.               
                //var result = await database.SqlQuery<int>("dbo.Geodist @p0, @p1", 2 ,3 ).FirstOrDefaultAsync();
                var result = await database.SqlQuery<SharingSpace>("dbo.Geodist @p0, @p1", Settings.GetUserId(User), int.Parse(distance)).ToListAsync();

                // Log the result.
                //Services.Log.Info(string.Format("{0} items set to 'complete'.", result.ToString()));
                //Debug.WriteLine();

                return result;

                //return StatusCode(HttpStatusCode.OK);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return null;
                //return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet, Authorize]
        [Route("api/custom/ss2/{sharingSpaceId}")]
        public Task<string> GetUserBySharingSpace(string sharingSpaceId)
        {
            string currentUserId = Settings.GetUserId(User);
            string userId = context.SharingSpaces.Where(e => e.Id == sharingSpaceId)
                                                 .Select(e => e.UserId)
                                                 .FirstOrDefault();
            Task<string> user;

            if (userId == currentUserId)
            {
                user = Task<string>.Factory.StartNew(() => "You");
            }
            else
            {
                string name = context.Users.Where(e => e.Id == userId)
                                           .Select(e => e.GivenName + " " + e.Surname)
                                           .FirstOrDefault();
                user = Task<string>.Factory.StartNew(() => name);
            }

            return user;
        }

        [HttpGet, Authorize]
        [Route("api/custom/ss/{sharingSpaceId}")]
        public IQueryable<string> VerifyUserParticipation(string sharingSpaceId)
        {
            var userId = Settings.GetUserId(User);
            var result = context.Events.Join(context.Constraints,
                                                e => e.ConstraintId,
                                                c => c.Id,
                                                (e, c) => new { e.SharingSpaceId, c.Operator, c.Value })
                                       .Where(o => o.SharingSpaceId == sharingSpaceId && o.Operator == "participant" && o.Value == userId)
                                       .Select(s => s.SharingSpaceId)
                                       .AsQueryable();

            return result;
        }

        [HttpGet, Authorize]
        [Route("api/custom/user/{sharingSpaceId}")]
        public IQueryable<string> VerifyOnwershipOfEvent(string sharingSpaceId)
        {
            var userId = Settings.GetUserId(User);
            var result = context.SharingSpaces
                                .Where(s => s.Id == sharingSpaceId && s.UserId == userId)
                                .Select(e => e.Id)
                                .AsQueryable();
            return result;
        }

        [HttpGet, Authorize]
        [Route("api/custom/dimension")]
        public IQueryable<string> GetDimensionId()
        {
            string sharingSpaceId = GetParameter(Request, "sharingSpaceId"),
                   topic = GetParameter(Request, "topic");

            var userId = Settings.GetUserId(User);
            var result = context.Events.Where(e => e.SharingSpaceId == sharingSpaceId)
                                       .Join(context.Dimensions, e => e.DimensionId, d => d.Id, (e, d) => new { d.Id, d.Label })
                                       .Where(d => d.Label == topic).Select(a => a.Id)
                                       .AsQueryable();
            return result;
        }

        /// <summary>
        /// Retrieve parameters sent with the request and throws exception if the parameter
        /// does not exist in the GET request. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetParameter(HttpRequestMessage request, string name)
        {
            var queryParams = request.GetQueryNameValuePairs().Where(kv => kv.Key == name).ToList();
            if (queryParams.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return queryParams[0].Value;
        }
    }
}
