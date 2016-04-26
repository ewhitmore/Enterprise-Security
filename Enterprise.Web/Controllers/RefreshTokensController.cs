using System.Web.Http;
using Enterprise.Web.Services;

namespace Enterprise.Web.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : ApiController
    {

        public ISecurityService SecurityService { get; set; }

        [Authorize(Users = "Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(SecurityService.GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult Delete(string referenceId)
        {
            var result = SecurityService.RemoveRefreshToken(referenceId);

            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");
        }
    }
}
