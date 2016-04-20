using System.Linq;
using Autofac;
using Enterprise.Persistence.Model;
using Enterprise.Web;
using Enterprise.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.AspNet.Identity;
using NHibernate.Context;


namespace Enterprise.Persistence.Tests.IntegrationTests
{
    [TestClass]
    public class SecurityServiceIntegrationTests
    {
        public ISecurityService SecurityService { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }

        [TestInitialize]
        public void Init()
        {
            TestingSessionFactory.CreateSessionFactory("thread_static"); // Create a thread_static NHibernate Session
            AutofacConfig.RegisterAutofac(TestingSessionFactory.SessionFactory); // Send session to IoC Container
            CurrentSessionContext.Bind(TestingSessionFactory.Session); // Bind to Unit Test Context
            SecurityService = AutofacConfig.Container.Resolve<ISecurityService>();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(TestingSessionFactory.Session));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(TestingSessionFactory.Session));
        }

        [TestCleanup]
        public void CleanUp()
        {
            HibernateConfig.Dispose();  // Release Session
        }

        [TestMethod]
        public void SecurityService_CreateUser_ReturnTrue()
        {
            // Arrange
            const string email = "the@email.local";
            const string username = "theUserName";
            const string password = "thePass";

            // Act
            var result = SecurityService.CreateUser(email, username, password);
            var user = SecurityService.Session.CreateCriteria<ApplicationUser>().List<ApplicationUser>().First();

            // Assert
            Assert.AreEqual(email, user.Email);
            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public void SecurityService_CreateUser_ReturnFalse()
        {
            // Arrange
            const string email = "the@email.local";
            const string username = "";
            const string password = "";

            // Act
            var result = SecurityService.CreateUser(email, username, password);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Passwords must be at least 6 characters.", result.Errors.First());
        }

        [TestMethod]
        public void SecurityService_InitalizeSecurity_ReturnTrue()
        {
            // Arrange
            const string username = "Admin";

            // Act
            SecurityService.InitalizeSecurity();
            var results = SecurityService.Session.CreateCriteria<ApplicationUser>().List<ApplicationUser>().First(x => x.UserName == username);

            // Assert
            Assert.IsTrue(RoleManager.RoleExists("SysAdmin"));
            Assert.AreEqual(username, results.UserName);
        }

        [TestMethod]
        public void SecurityService_UserExists_ReturnsTrue()
        {
            // Arrange
            const string email = "the@email.local";
            const string username = "theUserName";
            const string password = "thePass";

            // Act
            SecurityService.CreateUser(email, username, password);
            var results = SecurityService.UserExists(username);

            // Assert
            Assert.IsTrue(results);
        }

        [TestMethod]
        public void SecurityService_UserExists_ReturnsFalse()
        {
            // Arrange
            const string username = "NotAUser";

            // Act
            var results = SecurityService.UserExists(username);

            // Assert
            Assert.IsFalse(results);
        }

        [TestMethod]
        public void SecurityService_CreateRole_ReturnTrue()
        {
            // Arrange 
            const string roleName = "PowerUser";

            // Act
            var result = SecurityService.CreateRole(roleName);

            // Assert
            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public void SecurityService_CreateRole_ReturnFalse()
        {
            // Arrange 
            const string roleName = "";

            // Act
            var result = SecurityService.CreateRole(roleName);

            // Assert
            Assert.IsFalse(result.Succeeded);
        }
    }
}
