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
    public class SharingSpaceController : TableController<SharingSpace>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<SharingSpace>(context, Request, enableSoftDelete: true);
        }

        // GET tables/SharingSpace
        public IQueryable<SharingSpace> GetAllSharingSpace()
        {
            return Query(); 
        }

        // GET tables/SharingSpace/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<SharingSpace> GetSharingSpace(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/SharingSpace/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<SharingSpace> PatchSharingSpace(string id, Delta<SharingSpace> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/SharingSpace
        public async Task<IHttpActionResult> PostSharingSpace(SharingSpace item)
        {
            SharingSpace current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/SharingSpace/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSharingSpace(string id)
        {
             return DeleteAsync(id);
        }
    }
}
