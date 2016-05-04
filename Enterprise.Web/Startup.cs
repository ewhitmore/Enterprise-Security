using System;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Enterprise.Web.Security;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Enterprise.Web.Startup))]

namespace Enterprise.Web
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            // Configure IoC Container
            AutofacConfig.RegisterAutofac();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(AutofacConfig.Container);
            app.UseAutofacMiddleware(AutofacConfig.Container);

            // Configure Owin
            ConfigureOAuth(app);

            // Register Web API routes
            WebApiConfig.Register(config);

            // Try to avoid CORS issues with the browser
            app.UseCors(CorsOptions.AllowAll);

            // Apply settings to WebAPI
            app.UseWebApi(config);
            
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            // Use cookies to hold tokens
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(60),
                Provider = AutofacConfig.Container.Resolve<IOAuthAuthorizationServerProvider>(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
               
            };

            // Force app to use OWIN/OAuthBearer Tokens for authentication overriding existing auth system
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
            
        }
    }
}
