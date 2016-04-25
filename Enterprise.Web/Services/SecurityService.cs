using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enterprise.Model;
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




        // FROM OTHER PROJECT
        public IdentityResult RegisterUser(ApplicationUser applicationUser)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = applicationUser.UserName
                
                
            };

            var result = UserManager.Create(user, applicationUser.Password);

            return result;
        }

        public IdentityUser FindUser(string userName, string password)
        {
            IdentityUser user = UserManager.Find(userName, password);

            return user;
        }

        public Client FindClient(string clientId)
        {
            //var client = _ctx.Clients.Find(clientId);
            var client = Session.QueryOver<Client>().Where(c => c.Id == clientId).SingleOrDefault();

            return client;
        }

        public RefreshToken AddRefreshToken(RefreshToken token)
        {

            //var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();
            var existingToken = Session.QueryOver<RefreshToken>().Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                RemoveRefreshToken(existingToken);
            }

            //_ctx.RefreshTokens.Add(token);
            Session.Save(token);
            return token;
            //return await _ctx.SaveChangesAsync() > 0;
        }

        public bool RemoveRefreshToken(string refreshTokenId)
        {
            //var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            //if (refreshToken != null)
            //{
            //    RemoveRefreshToken(refreshToken);
            //    return await _ctx.SaveChangesAsync() > 0;
            //}

            //return false;

            RemoveRefreshToken(FindRefreshToken(refreshTokenId));

            return true;
        }

        public void RemoveRefreshToken(RefreshToken refreshToken)
        {
            //_ctx.RefreshTokens.Remove(refreshToken);
            //return await _ctx.SaveChangesAsync() > 0;

            Session.Delete(refreshToken);

        }

        public RefreshToken FindRefreshToken(string refreshTokenId)
        {
            //var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return Session.QueryOver<RefreshToken>().Where(r => r.Id == refreshTokenId).SingleOrDefault(); 
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            //return CurrentSession.CreateCriteria(typeof(TEntity)).List<TEntity>();


            return new List<RefreshToken>(Session.CreateCriteria(typeof(RefreshToken)).List<RefreshToken>());
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await UserManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var result = await UserManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await UserManager.AddLoginAsync(userId, login);

            return result;
        }
    }
}