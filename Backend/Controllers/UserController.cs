using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Backend.DataObjects;
using Backend.Models;
using System.Security.Claims;
using System.Security.Principal;
using Backend.Helpers;
using System.Net;
using System.Web.Http.Description;

namespace Backend.Controllers
{
    public class UserController : TableController<User>
    {
        [Authorize]
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<User>(context, Request, enableSoftDelete: true);
        }

        public string UserId => IdentityProvider + "_" + NameIdentifier;

        public string NameIdentifier => ((ClaimsPrincipal)User).FindFirst(ClaimTypes.NameIdentifier).Value.Split(':')[1];

        public string IdentityProvider => ((ClaimsPrincipal)User).FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;

        // GET tables/User
        public IQueryable<User> GetAllUser()
        {
            return Query(); 
        }

        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<User> GetUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<User> PatchUser(string id, Delta<User> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/User
        //public async Task<IHttpActionResult> PostUser(User item)
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User item)
        {
            //item.Id = UserId;
            item.Id = Settings.GetUserId(User);
            //item.Email = Settings.GetUserEmail(User);
            try
            {
                User current = await InsertAsync(item);
                return Ok(current);
            }
            catch (System.Exception)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }
            //return CreatedAtRoute("Tables", new { id = current.Id }, current);
            
        }

        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUser(string id)
        {
             return DeleteAsync(id);
        }

    }
}
