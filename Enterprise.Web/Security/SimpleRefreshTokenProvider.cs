using System;
using System.Threading.Tasks;
using Enterprise.Model;
using Enterprise.Persistence;
using Enterprise.Web.Utils;
using Microsoft.Owin.Security.Infrastructure;
using NHibernate;

namespace Enterprise.Web.Security
{
    /// <summary>
    /// Called by startup class to handle Refresh Tokens
    /// </summary>
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider, IDisposable
    {
        public ISession Session { get; set; }

        //public ISecurityService SecurityService { get; set; }

        public SimpleRefreshTokenProvider()
        {
            Session = HibernateConfig.CreateSessionFactory("EnterpriseSecurityDatabase", "web").OpenSession();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new RefreshToken()
            {
                Id = Helper.GetHash(refreshTokenId),
                //Id = Guid.NewGuid(),
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = AddRefreshToken(token);

            if (result != null)
            {
                context.SetToken(refreshTokenId);
            }


        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helper.GetHash(context.Token);


            //todo: thji
            //var refreshToken = Session.QueryOver<RefreshToken>().Where(r => r.Id.ToString() == hashedTokenId).SingleOrDefault();
            var refreshToken = Session.QueryOver<RefreshToken>().Where(x => x.Id == hashedTokenId).SingleOrDefault<RefreshToken>();




            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                Session.Delete(refreshToken);
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

        public void Dispose()
        {
            Session.Dispose();
        }


        private RefreshToken AddRefreshToken(RefreshToken token)
        {
            //var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();
            var existingToken = Session.QueryOver<RefreshToken>().Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {

                Session.Delete(existingToken);
                
            }

            //_ctx.RefreshTokens.Add(token);
            Session.Save(token);
            Session.Flush();
            return token;
            //return await _ctx.SaveChangesAsync() > 0;
        }
    }
}