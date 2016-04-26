using System;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Integration.WebApi;
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
        private ISession Session { get; set; }

        public SimpleRefreshTokenProvider()
        {
            // Todo: Can we do this in the IoC Container?
            // Create database session
            Session = HibernateConfig.CreateSessionFactory("EnterpriseSecurityDatabase", "web").OpenSession();

        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            // Pull ClientId from OWIN context
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            // Reject requests without ClientId
            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            // Convert GUID to STRING
            var refreshTokenId = Guid.NewGuid().ToString("n");

            // Get Lifetime from context which comes from database for this app type
            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            // Create RefreshToken
            var token = new RefreshToken()
            {
                Id = Helper.GetHash(refreshTokenId),
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            // Set OWIN contxt properties
            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            // Add ProtextedTicket to property so it can get into the database
            token.ProtectedTicket = context.SerializeTicket();

            // Add token to database
            var result = AddRefreshToken(token);

            // If our token saves to the database set it to the context
            if (result != null)
            {
                context.SetToken(refreshTokenId);
            }


        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            // Get Origin from context and add it to the response headers to avoid CORS problems on client
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            // Get Hashed token that matches Refresh Token ID
            string hashedTokenId = Helper.GetHash(context.Token);

            // Get Token if it exists
            var refreshToken = Session.QueryOver<RefreshToken>().Where(x => x.Id == hashedTokenId).SingleOrDefault<RefreshToken>();

            // If token exists pull protectedTicket to make new token and remove current token from database
            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                Session.Delete(refreshToken);
                Session.Flush();
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
            // Get an existing token if it exists
            var existingToken = Session.QueryOver<RefreshToken>().Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            // If it exists delete it
            if (existingToken != null)
            {
                Session.Delete(existingToken);
            }

            // Save new token
            Session.Save(token);

            // Commit to database
            Session.Flush();
            return token;

        }
    }
}