using System.Collections.Generic;
using Enterprise.Model;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.AspNet.Identity;

namespace Enterprise.Web.Services
{
    public interface ISecurityService
    {
        ISession Session { get; set; }
        /// <summary>
        /// Create a User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        IdentityResult CreateUser(string email, string username, string password);

        /// <summary>
        /// Create a Role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        IdentityResult CreateRole(string roleName);

        /// <summary>
        /// Setup Inital Admin user with password "TheAdminPass".  Will only run if Admin account doesn't already exist
        /// </summary>
        void InitalizeSecurity();

        /// <summary>
        /// Returns true if the user exists in the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool UserExists(string username);

        /// <summary>
        /// Update password for a given username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IdentityResult UpdatePassword(string username, string password);

        IdentityUser FindUser(string userName, string password);
        Client FindClient(string clientId);
        RefreshToken AddRefreshToken(RefreshToken token);
        bool RemoveRefreshToken(string referenceId);
        void RemoveRefreshToken(RefreshToken refreshToken);
        RefreshToken FindRefreshToken(string referenceId);
        List<RefreshToken> GetAllRefreshTokens();
  
    }
}