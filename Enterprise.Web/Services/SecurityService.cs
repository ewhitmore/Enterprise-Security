using System;
using Enterprise.Persistence.Model;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.AspNet.Identity;


namespace Enterprise.Web.Services
{

    // http://www.primaryobjects.com/2015/05/08/token-based-authentication-for-web-service-apis-in-c-mvc-net/

    public sealed class SecurityService : ISecurityService
    {
        public ISession Session { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        private RoleManager<IdentityRole> RoleManager { get; set; }

        public SecurityService(ISession session)
        {
            Session = session;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(session));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(session));
        }

        /// <summary>
        /// Create a User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public IdentityResult CreateUser(string email, string username, string password)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = username
            };

            return UserManager.Create(user, password);
        }

        /// <summary>
        /// Create a Role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityResult CreateRole(string roleName)
        {
            return RoleManager.Create(new IdentityRole {Name = roleName });
        }

        /// <summary>
        /// Setup Inital Admin user with password "TheAdminPass".  Will only run if Admin account doesn't already exist
        /// </summary>
        public void InitalizeSecurity()
        {
            if (!UserExists("Admin"))
            {
                var createUserResults = CreateUser("admin@localhost", "Admin", "TheAdminPass");

                if (!createUserResults.Succeeded)
                {
                    throw new Exception(string.Concat(createUserResults.Errors));
                }

                var createRoleResults = CreateRole("SysAdmin");

                if (!createRoleResults.Succeeded)
                {
                    throw new Exception(string.Concat(createRoleResults.Errors));
                }

                var user = UserManager.FindByName("Admin");
                UserManager.AddToRole(user.Id, "SysAdmin");
            }
        }

        /// <summary>
        /// Returns true if the user exists in the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool UserExists(string username)
        {
            return UserManager.FindByName(username) != null;
        }
    }
}