using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Backend.Models;
using System.Linq;
using Backend.DataObjects;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [MobileAppController]
    public class DimDatatypeController : ApiController
    {
        MobileServiceContext context;

        public DimDatatypeController() : base()
        {
            context = new MobileServiceContext();
        }
        // GET api/DimDatatype
        [HttpGet]
        public IQueryable<DimDatatype> Get()
        {
            return context.DimDatatypes;
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAsync(DimDatatype item)
        {
            try
            {
                context.DimDatatypes.Add(item);
                await context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.Created);
            }
            catch (System.Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }
    }
}
