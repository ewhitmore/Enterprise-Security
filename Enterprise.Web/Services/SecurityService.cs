using System.Collections.Generic;
using System.Linq;
using Enterprise.Model;
using Enterprise.Persistence.Dao;
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
        public IRefreshTokenDao RefreshTokenDao { get; set; }
        public IClientDao ClientDao { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        private RoleManager<IdentityRole> RoleManager { get; set; }

        public SecurityService(ISession session, IRefreshTokenDao refreshTokenDao)
        {
            Session = session;
            RefreshTokenDao = refreshTokenDao;
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
        public IdentityResult InitalizeSecurity()
        {
            if (UserExists("Admin")) return new IdentityResult("Admin already exists");

            var createUserResults = CreateUser("admin@localhost", "Admin", "TheAdminPass");

            if (!createUserResults.Succeeded)
            {
                return new IdentityResult(string.Concat(createUserResults.Errors, ","));
            }

            var createRoleResults = CreateRole("SysAdmin");

            if (!createRoleResults.Succeeded)
            {
                return new IdentityResult(string.Concat(createRoleResults.Errors, ","));
            }

            var user = UserManager.FindByName("Admin");
            return UserManager.AddToRole(user.Id, "SysAdmin");
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

        /// <summary>
        /// Update password for a given username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IdentityResult UpdatePassword(string username, string password)
        {
            var user = UserManager.FindByName(username);
            UserManager.RemovePassword(user.Id);
            return UserManager.AddPassword(user.Id, password);
        }

        /// <summary>
        /// Find user by username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IdentityUser FindUser(string userName, string password)
        {
            return UserManager.Find(userName, password);
        }

        /// <summary>
        /// Find client by client identifier
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Client FindClient(string clientId)
        {
            return ClientDao.FindAll().SingleOrDefault(c => c.ClientId.ToString() == clientId);
        }

        /// <summary>
        /// Replace refresh token if one exists otherwise add
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public RefreshToken AddRefreshToken(RefreshToken token)
        {
            var existingToken = RefreshTokenDao.FindAll()
                    .FirstOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);


            if (existingToken != null)
            {
                RemoveRefreshToken(existingToken);
            }

            Session.Save(token);
            return token;
        }

        /// <summary>
        /// Returns true if toek was removed from database
        /// </summary>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        public bool RemoveRefreshToken(string referenceId)
        {
            var refreshToken = FindRefreshToken(referenceId);

            if (refreshToken == null) return false;

            RemoveRefreshToken(refreshToken);
            return true;
        }

        /// <summary>
        /// Delete Refresh Token from database
        /// </summary>
        /// <param name="refreshToken"></param>
        public void RemoveRefreshToken(RefreshToken refreshToken)
        {
            RefreshTokenDao.Delete(refreshToken);
        }

        /// <summary>
        /// Get Refresh Token by ReferenceId
        /// </summary>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        public RefreshToken FindRefreshToken(string referenceId)
        {
            return RefreshTokenDao.FindAll().FirstOrDefault(r => r.ReferenceId == referenceId);
        }

        /// <summary>
        /// Get All Refresh Tokens
        /// </summary>
        /// <returns></returns>
        public List<RefreshToken> GetAllRefreshTokens()
        {
            return RefreshTokenDao.GetAll().ToList();
        }
    }
}