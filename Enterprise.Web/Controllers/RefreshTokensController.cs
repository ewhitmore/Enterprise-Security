using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            //return Ok(_repo.GetAllRefreshTokens());

            return Ok(SecurityService.GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            //var result = await _repo.RemoveRefreshToken(tokenId);
            var result = SecurityService.RemoveRefreshToken(tokenId);

            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");

        }
    }
}
