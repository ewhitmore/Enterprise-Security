using System.Linq;
using System.Security.Principal;
using System.Threading;
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

        [TestInitialize]
        public void Init()
        {
            TestingSessionFactory.CreateSessionFactory("thread_static"); // Create a thread_static NHibernate Session
            AutofacConfig.RegisterAutofac(TestingSessionFactory.SessionFactory); // Send session to IoC Container
            CurrentSessionContext.Bind(TestingSessionFactory.Session); // Bind to Unit Test Context
            SecurityService = AutofacConfig.Container.Resolve<ISecurityService>();

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
            SecurityService.CreateUser(email, username, password);
            var results = SecurityService.Session.CreateCriteria<ApplicationUser>().List<ApplicationUser>().First();

            // Assert
            Assert.AreEqual(email, results.Email);

        }

        [TestMethod]
        public void SecurityService_InitalizeSecurity_ReturnTrue()
        {

            // Arrange
            var session = TestingSessionFactory.Session;
            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(session));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(session));
            const string username = "Admin";

            // Act
            SecurityService.InitalizeSecurity();
            var results = SecurityService.Session.CreateCriteria<ApplicationUser>().List<ApplicationUser>().First(x => x.UserName == username);

            // Assert
            Assert.IsTrue(roleManager.RoleExists("SysAdmin"));
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


    }
}
