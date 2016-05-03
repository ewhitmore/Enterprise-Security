using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Enterprise.Model;
using Enterprise.Web.Models;
using Enterprise.Web.Services;
using Enterprise.Web.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Enterprise.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/account")]
    public class AccountController : ApiController
    {
        public ISecurityService SecurityService { get; set; }

        [ValidateModelState]
        [HttpPost]
        [Route("CreateUser")]
        public IHttpActionResult CreateUser(UserModel userModel)
        {
            var result = SecurityService.CreateUser(userModel.EmailAddress, userModel.UserName, userModel.Password);
            var errorResult = GetErrorResult(result);

            return errorResult ?? Ok();
        }

        [ValidateModelState]
        [HttpPut]
        [Route("UpdatePassword")]
        public IHttpActionResult UpdatePassword(UserModel userModel)
        {
            var result = SecurityService.UpdatePassword(userModel.UserName, userModel.Password);
            var errorResult = GetErrorResult(result);

            return errorResult ?? Ok();
        }

        [HttpGet]
        [Route("GetRefreshTokens")]
        public IList<RefreshToken> GetRefreshTokens()
        {
            return SecurityService.GetAllRefreshTokens();
        }



        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
        #endregion

    }
}
