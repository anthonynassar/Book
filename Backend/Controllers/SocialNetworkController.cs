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
    public class SocialNetworkController : TableController<SocialNetwork>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<SocialNetwork>(context, Request);
        }

        // GET tables/SocialNetwork
        public IQueryable<SocialNetwork> GetAllSocialNetwork()
        {
            return Query(); 
        }

        // GET tables/SocialNetwork/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<SocialNetwork> GetSocialNetwork(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/SocialNetwork/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<SocialNetwork> PatchSocialNetwork(string id, Delta<SocialNetwork> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/SocialNetwork
        public async Task<IHttpActionResult> PostSocialNetwork(SocialNetwork item)
        {
            SocialNetwork current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/SocialNetwork/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSocialNetwork(string id)
        {
             return DeleteAsync(id);
        }
    }
}
