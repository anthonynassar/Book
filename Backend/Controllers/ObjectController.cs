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
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Object>(context, Request);
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
