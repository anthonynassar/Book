using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Backend.DataObjects;
using Backend.Models;

namespace Backend.Controllers
{
    public class GranularityController : TableController<Granularity>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Granularity>(context, Request);
        }

        // GET tables/Granularity
        public IQueryable<Granularity> GetAllGranularity()
        {
            return Query(); 
        }

        // GET tables/Granularity/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Granularity> GetGranularity(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Granularity/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Granularity> PatchGranularity(string id, Delta<Granularity> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Granularity
        public async Task<IHttpActionResult> PostGranularity(Granularity item)
        {
            Granularity current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Granularity/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteGranularity(string id)
        {
             return DeleteAsync(id);
        }
    }
}
