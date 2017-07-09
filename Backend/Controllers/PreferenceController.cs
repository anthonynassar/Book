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
    public class PreferenceController : TableController<Preference>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Preference>(context, Request, enableSoftDelete: true);
        }

        // GET tables/Preference
        public IQueryable<Preference> GetAllPreference()
        {
            return Query(); 
        }

        // GET tables/Preference/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Preference> GetPreference(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Preference/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Preference> PatchPreference(string id, Delta<Preference> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Preference
        public async Task<IHttpActionResult> PostPreference(Preference item)
        {
            Preference current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Preference/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePreference(string id)
        {
             return DeleteAsync(id);
        }
    }
}
