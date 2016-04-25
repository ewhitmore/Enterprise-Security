using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace Enterprise.Web.Security
{
    public interface ISimpleAuthorizationServerProvider
    {
        Func<OAuthMatchEndpointContext, Task> OnMatchEndpoint { get; set; }
        Func<OAuthValidateClientRedirectUriContext, Task> OnValidateClientRedirectUri { get; set; }
        Func<OAuthValidateClientAuthenticationContext, Task> OnValidateClientAuthentication { get; set; }
        Func<OAuthValidateAuthorizeRequestContext, Task> OnValidateAuthorizeRequest { get; set; }
        Func<OAuthValidateTokenRequestContext, Task> OnValidateTokenRequest { get; set; }
        Func<OAuthGrantAuthorizationCodeContext, Task> OnGrantAuthorizationCode { get; set; }
        Func<OAuthGrantResourceOwnerCredentialsContext, Task> OnGrantResourceOwnerCredentials { get; set; }
        Func<OAuthGrantClientCredentialsContext, Task> OnGrantClientCredentials { get; set; }
        Func<OAuthGrantRefreshTokenContext, Task> OnGrantRefreshToken { get; set; }
        Func<OAuthGrantCustomExtensionContext, Task> OnGrantCustomExtension { get; set; }
        Func<OAuthAuthorizeEndpointContext, Task> OnAuthorizeEndpoint { get; set; }
        Func<OAuthTokenEndpointContext, Task> OnTokenEndpoint { get; set; }
        Func<OAuthAuthorizationEndpointResponseContext, Task> OnAuthorizationEndpointResponse { get; set; }
        Func<OAuthTokenEndpointResponseContext, Task> OnTokenEndpointResponse { get; set; }
        Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context);
        Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context);
        Task GrantRefreshToken(OAuthGrantRefreshTokenContext context);
        Task TokenEndpoint(OAuthTokenEndpointContext context);
        Task MatchEndpoint(OAuthMatchEndpointContext context);
        Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context);
        Task ValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context);
        Task ValidateTokenRequest(OAuthValidateTokenRequestContext context);
        Task GrantAuthorizationCode(OAuthGrantAuthorizationCodeContext context);
        Task GrantClientCredentials(OAuthGrantClientCredentialsContext context);
        Task GrantCustomExtension(OAuthGrantCustomExtensionContext context);
        Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context);
        Task AuthorizationEndpointResponse(OAuthAuthorizationEndpointResponseContext context);
        Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context);
    }
}