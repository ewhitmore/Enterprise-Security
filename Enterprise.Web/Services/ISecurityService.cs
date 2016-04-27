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
        IdentityResult InitalizeSecurity();

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

        /// <summary>
        /// Find user by username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IdentityUser FindUser(string userName, string password);

        /// <summary>
        /// Find client by client identifier
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Client FindClient(string clientId);

        /// <summary>
        /// Replace refresh token if one exists otherwise add
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        RefreshToken AddRefreshToken(RefreshToken token);

        /// <summary>
        /// Returns true if toek was removed from database
        /// </summary>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        bool RemoveRefreshToken(string referenceId);

        /// <summary>
        /// Delete Refresh Token from database
        /// </summary>
        /// <param name="refreshToken"></param>
        void RemoveRefreshToken(RefreshToken refreshToken);

        /// <summary>
        /// Get Refresh Token by ReferenceId
        /// </summary>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        RefreshToken FindRefreshToken(string referenceId);

        /// <summary>
        /// Get All Refresh Tokens
        /// </summary>
        /// <returns></returns>
        List<RefreshToken> GetAllRefreshTokens();
    }
}