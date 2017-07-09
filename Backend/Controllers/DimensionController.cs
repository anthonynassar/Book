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
    public class DimensionController : TableController<Dimension>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Dimension>(context, Request, enableSoftDelete: true);
        }

        // GET tables/Dimension
        public IQueryable<Dimension> GetAllDimension()
        {
            return Query(); 
        }

        // GET tables/Dimension/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Dimension> GetDimension(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Dimension/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Dimension> PatchDimension(string id, Delta<Dimension> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Dimension
        public async Task<IHttpActionResult> PostDimension(Dimension item)
        {
            Dimension current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Dimension/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDimension(string id)
        {
             return DeleteAsync(id);
        }
    }
}
