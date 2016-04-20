using Microsoft.AspNet.Identity;
using NHibernate;

namespace Enterprise.Web.Services
{
    public interface ISecurityService
    {
        /// <summary>
        /// NHibernate Database Session
        /// </summary>
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
    }
}