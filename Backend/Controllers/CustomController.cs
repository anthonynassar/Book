using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Backend.Models;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using Backend.Helpers;
using Backend.DataObjects;

namespace Backend.Controllers
{
    [MobileAppController]
    [Authorize]
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

        public class SomeType
        {
            public int a1;
            public int a2;
        }
    
    }
}
