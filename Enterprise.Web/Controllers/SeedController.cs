using System.Web.Http;
using Enterprise.Web.Services;

namespace Enterprise.Web.Controllers
{
    [RoutePrefix("api/v1/seed")]
    public class SeedController : ApiController
    {
        public ISeedService SeedService { get; set; }
        public SeedController(ISeedService seedService)
        {
            SeedService = seedService;
        }

        [HttpGet]
        [Route("seed")]
        public IHttpActionResult Seed()
        {
            SeedService.Seed();
            return Ok("DB Seeding Complete");
        }

    }
}
