using System;
using System.Collections.Generic;
using Enterprise.Persistence.Model;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.AspNet.Identity;
using NHibernate.Mapping.ByCode.Impl;


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
        /// Create User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void CreateUser(string email, string username, string password)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = username
            };

           UserManager.Create(user, password);
        }

        /// <summary>
        /// Setup Inital Admin user with password "TheAdminPass".  Will only run if Admin account doesn't already exist
        /// </summary>
        public void InitalizeSecurity()
        {
            if (!UserExists("Admin"))
            {
                CreateUser("admin@localhost", "Admin", "TheAdminPass");
                RoleManager.Create(new IdentityRole {Name = "SysAdmin"});
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