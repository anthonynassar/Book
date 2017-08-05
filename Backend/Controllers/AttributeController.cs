using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Backend.Models;
using Backend.DataObjects;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [MobileAppController]
    public class AttributeController : ApiController
    {
        MobileServiceContext context;

        public AttributeController() : base()
        {
            context = new MobileServiceContext();
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAsync(Attribute item)
        {
            try
            {
                context.Attributes.Add(item);
                await context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.Created);
            }
            catch (System.Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        // GET api/Attribute
        public string Get()
        {
            return "Hello from custom controller!";
        }
    }
}
