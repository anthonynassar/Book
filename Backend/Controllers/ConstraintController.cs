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
    public class ConstraintController : TableController<Constraint>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Constraint>(context, Request);
        }

        // GET tables/Constraint
        public IQueryable<Constraint> GetAllConstraint()
        {
            return Query(); 
        }

        // GET tables/Constraint/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Constraint> GetConstraint(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Constraint/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Constraint> PatchConstraint(string id, Delta<Constraint> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Constraint
        public async Task<IHttpActionResult> PostConstraint(Constraint item)
        {
            Constraint current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Constraint/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteConstraint(string id)
        {
             return DeleteAsync(id);
        }
    }
}
