using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Linq;
using Backend.DataObjects;
using System.Threading.Tasks;
using Backend.Models;
using System.Net;

namespace Backend.Controllers
{
    [MobileAppController]
    public class EventController : ApiController
    {
        MobileServiceContext context;

        public EventController() : base()
        {
            context = new MobileServiceContext();
        }
        // GET api/Event
        //[Route(Name = "api/Event")]
        [HttpGet]
        public IQueryable<Event> Get()
        {
            return context.Events;
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAsync(Event item)
        {
            try
            {
                context.Events.Add(item);
                await context.SaveChangesAsync();


                return StatusCode(HttpStatusCode.Created);
            }
            catch (System.Exception)
            {

                return StatusCode(HttpStatusCode.BadRequest);
            }
            
        }
        // GET tables/Event
        //public IQueryable<Event> GetAllEvent()
        //{
        //    return Query();
        //}



        //// POST tables/Event
        //public async Task<IHttpActionResult> PostEvent(Event item)
        //{
        //    Event current = await InsertAsync(item);
        //    return CreatedAtRoute("Tables", new { sharingSpace = current.SharingSpaceId, dimension = current.DimensionId, constraint = current.ConstraintId }, current);
        //}
    }
}
