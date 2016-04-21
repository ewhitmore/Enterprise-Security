using System.Web.Http;
using Enterprise.Web.Services;

namespace Enterprise.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/v1/seed")]
    public class SeedController : ApiController
    {
        public ISeedService SeedService { get; set; }

        public ISecurityService SecurityService { get; set; }

        public SeedController(ISeedService seedService, ISecurityService securityService)
        {
            SeedService = seedService;
            SecurityService = securityService;
        }

        [HttpGet]
        [Route("seed")]
        public IHttpActionResult Seed()
        {
            SeedService.Seed();
            return Ok("DB Seeding Complete");
        }

        [HttpGet]
        [Route("init")]
        public void Init()
        {
            SecurityService.InitalizeSecurity();
        }


    }
}
