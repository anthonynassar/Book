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
    public class DatatypeController : TableController<Datatype>
    {
        MobileServiceContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Datatype>(context, Request);
        }

        // GET tables/Datatype
        public IQueryable<Datatype> GetAllDatatype()
        {
            return Query(); 
        }

        // GET tables/Datatype/Special/{sharingSpaceId}
        [HttpGet]
        [Route("tables/datatype/special/{sharingSpaceId}")]
        public IQueryable<SpecialDatatype> GetDatatypesBySharingSpace(string sharingSpaceId)
        {
            var result = context.Events.Where(e => e.SharingSpaceId == sharingSpaceId).Join(context.Dimensions,
            e => e.DimensionId,
            d => d.Id,
            (e, d) => new { e.DimensionId, d.Label }).Join(context.DimDatatypes,
                                                        d => d.DimensionId,
                                                        dd => dd.DimensionId,
                                                        (d, dd) => new SpecialDatatype { DatatypeId = dd.DatatypeId, Label = d.Label })
                                                      .Distinct();

            return result.AsQueryable();
        }

        // GET tables/Datatype/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Datatype> GetDatatype(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Datatype/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Datatype> PatchDatatype(string id, Delta<Datatype> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Datatype
        public async Task<IHttpActionResult> PostDatatype(Datatype item)
        {
            Datatype current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Datatype/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDatatype(string id)
        {
             return DeleteAsync(id);
        }
    }

    public class SpecialDatatype
    {
        public string DatatypeId { get; set; }
        public string Label { get; set; }
    }
}
