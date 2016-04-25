using System;
using System.Linq;
using System.Threading.Tasks;
using Enterprise.Model;
using Enterprise.Persistence.Model;
using Enterprise.Web.Services;
using Enterprise.Web.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Infrastructure;
using NHibernate;
using NHibernate.AspNet.Identity;

namespace Enterprise.Web.Security
{
    public class SimpleRefreshTokenProviderService : IAuthenticationTokenProvider
    {
        public ISession Session { get; set; }

        public ISecurityService SecurityService { get; set; }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            //using (AuthRepository _repo = new AuthRepository())
            //{
            //    var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            //    var token = new RefreshToken()
            //    {
            //        Id = Helper.GetHash(refreshTokenId),
            //        ClientId = clientid,
            //        Subject = context.Ticket.Identity.Name,
            //        IssuedUtc = DateTime.UtcNow,
            //        ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            //    };

            //    context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            //    context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            //    token.ProtectedTicket = context.SerializeTicket();

            //    var result = await _repo.AddRefreshToken(token);

            //    if (result)
            //    {
            //        context.SetToken(refreshTokenId);
            //    }

            //}

            //todo: fix this?
           
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RefreshToken()
                {
                    Id = Helper.GetHash(refreshTokenId),
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Session));
            var result = SecurityService.AddRefreshToken(token);

                if (result.Id.Any())
                {
                    context.SetToken(refreshTokenId);
                }

         
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helper.GetHash(context.Token);

            //using (AuthRepository _repo = new AuthRepository())
            //{
            //    var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

            //    if (refreshToken != null)
            //    {
            //        //Get protectedTicket from refreshToken class
            //        context.DeserializeTicket(refreshToken.ProtectedTicket);
            //        var result = await _repo.RemoveRefreshToken(hashedTokenId);
            //    }
            //}

            var refreshToken = SecurityService.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = SecurityService.RemoveRefreshToken(hashedTokenId);
            }


        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}