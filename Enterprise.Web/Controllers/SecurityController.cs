using System.Web.Http;
using Enterprise.Web.Services;

namespace Enterprise.Web.Controllers
{
    [RoutePrefix("api/v1/security")]
    public class SecurityController : ApiController
    {

        public ISecurityService SecurityService { get; set; }

        public SecurityController(ISecurityService securityService)
        {
            SecurityService = securityService;
        }

        [Route("Init")]
        [HttpGet]
        public IHttpActionResult Init()
        {
            if (SecurityService.UserExists("Admin"))
            {
                return Ok("Admin User already exists - task aborted");
            }

            SecurityService.InitalizeSecurity();
            return Ok("Task Complete");
        }
    }
}
