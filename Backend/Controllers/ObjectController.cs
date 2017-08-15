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
    public class ObjectController : TableController<Object>
    {
        MobileServiceContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Object>(context, Request, enableSoftDelete: true);
        }

        // GET tables/Object
        public IQueryable<Object> GetAllObject()
        {
            return Query(); 
        }

        // GET tables/Object/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Object> GetObject(string id)
        {
            return Lookup(id);
        }

        // GET tables/Object/Special/{sharingSpaceId}
        [HttpGet]
        [Route("tables/object/special/{sharingSpaceId}")]
        public IQueryable<Object> GetObjectsBySharingSpace(string sharingSpaceId)
        {
            var result = context.Objects.Where(o => o.SharingSpaceId == sharingSpaceId);

            return result.AsQueryable();
        }

        // PATCH tables/Object/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Object> PatchObject(string id, Delta<Object> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Object
        public async Task<IHttpActionResult> PostObject(Object item)
        {
            Object current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Object/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteObject(string id)
        {
             return DeleteAsync(id);
        }
    }
}
