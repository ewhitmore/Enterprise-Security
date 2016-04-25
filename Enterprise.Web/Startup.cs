using System;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Enterprise.Web.Security;
using Enterprise.Web.Services;
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
            HttpConfiguration config = new HttpConfiguration();
            AutofacConfig.RegisterAutofac();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(AutofacConfig.Container);
            app.UseAutofacMiddleware(AutofacConfig.Container);

            ConfigureOAuth(app);

            
            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);


            
        }



        private void ConfigureOAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {

                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                //Provider = new SimpleAuthorizationServerProviderService(),
                Provider = AutofacConfig.Container.Resolve<IOAuthAuthorizationServerProvider>(),
                RefreshTokenProvider = new SimpleRefreshTokenProviderService()
               
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
            
        }
    }
}
