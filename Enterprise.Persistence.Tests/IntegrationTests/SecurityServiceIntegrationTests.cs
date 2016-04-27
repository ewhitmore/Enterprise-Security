using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Enterprise.Model;
using Enterprise.Persistence.Dao;
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
        public IClientDao ClientDao { get; set; }
        public IRefreshTokenDao RefreshTokenDao { get; set; }
        public ISecurityService SecurityService { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }

        [TestInitialize]
        public void Init()
        {
            TestingSessionFactory.CreateSessionFactory("thread_static"); // Create a thread_static NHibernate Session
            AutofacConfig.RegisterAutofac(TestingSessionFactory.SessionFactory); // Send session to IoC Container
            CurrentSessionContext.Bind(TestingSessionFactory.Session); // Bind to Unit Test Context
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(TestingSessionFactory.Session));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(TestingSessionFactory.Session));

            SecurityService = AutofacConfig.Container.Resolve<ISecurityService>();
            RefreshTokenDao = AutofacConfig.Container.Resolve<IRefreshTokenDao>();
            ClientDao = AutofacConfig.Container.Resolve<IClientDao>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            HibernateConfig.Dispose();  // Release Session
        }

        private void SeedDatabase()
        {

            #region refresh tokens
            var tokens = new List<RefreshToken>()
            {
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/uPCkmZrQIkFAjwFUUk2aDoToGyuGqpdNQ=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk",
                    Subject = "ewhitmore"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/uPCkmZrQIkFAjwFUUk2aDoToGyuGqpdNQ=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-7890789-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk",
                    Subject = "ewhitmore"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/12312312312312=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-79078907890-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk",
                    Subject = "new_user"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/12222111=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62G456456456hhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk",
                    Subject = "ewhitmore"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/gfhjfndb b=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk331237qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk",
                    Subject = "ewhitmore"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/asdfasdf=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-sdfgsdfgs-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk",
                    Subject = "ewhitmore"

                }
            };

            foreach (var refreshToken in tokens)
            {
                RefreshTokenDao.Save(refreshToken);
            }
            #endregion

            #region Client

            var clients = new List<Client>{

                new Client()
                {
                    ClientId = "ngAuthApp1",
                    Active = true,
                    AllowedOrigin = "*",
                    ApplicationType = ApplicationTypes.JavaScript,
                    Name = "AngularJS front-end Application",
                    RefreshTokenLifeTime = 7200,
                    Secret = "5YV7M1r981yoGhELyB84aC+444556++C1CtRM="
                },
                new Client()
                {
                    ClientId = "ngAuthApp2",
                    Active = true,
                    AllowedOrigin = "*",
                    ApplicationType = ApplicationTypes.JavaScript,
                    Name = "AngularJS front-end Application",
                    RefreshTokenLifeTime = 7200,
                    Secret = "1234433222+KiYksxZf1OY3++C1CtRM="
                },
                new Client()
                {
                    ClientId = "ngAuthAp3p",
                    Active = true,
                    AllowedOrigin = "*",
                    ApplicationType = ApplicationTypes.NativeConfidential,
                    Name = "AngularJS front-end Application",
                    RefreshTokenLifeTime = 1234,
                    Secret = "5YV7M1r981yoGhELyB84aC+sdfgsdfs++C1CtRM="
                }
            };

            foreach (var client in clients)
            {
                ClientDao.Save(client);
            }

            #endregion

            SecurityService.InitalizeSecurity();
            SecurityService.CreateUser("ewhitmore@localhost", "ewhitmore", "ThePassword");
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

        [TestMethod]
        public void SecurityService_UpdatePassword_ReturnTrue()
        {
            // Arrange
            const string email = "the@email.local";
            const string username = "username";
            const string oldPassword = "ThePassword";
            const string newPassword = "TheNEWPassword";

            // Act
            SecurityService.CreateUser(email, username, oldPassword);
            var result = SecurityService.UpdatePassword(username, newPassword);

            // Assert
            Assert.IsTrue(result.Succeeded);

        }

        [TestMethod]
        public void SecurityService_UpdatePassword_ReturnFalse()
        {
            // Arrange
            const string email = "the@email.local";
            const string username = "username";
            const string oldPassword = "ThePassword";
            const string newPassword = "";

            // Act
            SecurityService.CreateUser(email, username, oldPassword);
            var result = SecurityService.UpdatePassword(username, newPassword);

            // Assert
            Assert.IsFalse(result.Succeeded);

        }

        [TestMethod]
        public void SecurityService_FindUser_ReturnTrue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var user = SecurityService.FindUser("ewhitmore", "ThePassword");

            // Assert
            Assert.AreEqual("ewhitmore", user.UserName);
        }

        [TestMethod]
        public void SecurityService_FindUser_ReturnNull()
        {
            // Arrange
            SeedDatabase();

            // Act
            var user = SecurityService.FindUser("ewhitmore", "");

            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void SecurityService_FindClient_ReturnTrue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var client = SecurityService.FindClient("ngAuthApp1");

            // Assert
            Assert.AreEqual(client.Secret, "5YV7M1r981yoGhELyB84aC+444556++C1CtRM=");
        }

        [TestMethod]
        public void SecurityService_FindClient_ReturnNull()
        {
            // Arrange
            SeedDatabase();

            // Act
            var client = SecurityService.FindClient("noClient");

            // Assert
            Assert.IsNull(client);
        }

        [TestMethod]
        public void SecurityService_AddRefreshTokenExistingToken_ReturnTrue()
        {
            // Arrange
            SeedDatabase();

            var token = new RefreshToken()
            {
                ClientId = "ngAuthApp",
                ReferenceId = "qsKugCAf4/uPCkmZrQIkFAjwFUUk2aDoToGyuGqpdNQ=",
                IssuedUtc = new DateTime(2016, 07, 01, 15, 29, 25, 000),
                ExpiresUtc = new DateTime(2016, 08, 01, 15, 29, 25, 000),
                ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfy" +
                "W2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsT" + 
                "YgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGi" + 
                "rHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk",
                Subject = "ewhitmore"

            };

            // Act
            var newToken = SecurityService.AddRefreshToken(token);

            // Assert
            Assert.AreEqual(token.ReferenceId, newToken.ReferenceId);

        }

        [TestMethod]
        public void SecurityService_AddRefreshTokenNewToken_ReturnTrue()
        {
            // Arrange
            SeedDatabase();

            var token = new RefreshToken()
            {
                ReferenceId = "oFKegx4g3Ld0avGWZYKBY6LWlbKLlcxShJLvG8etg/I=",
                ClientId = "ngAuthApp",
                Subject = "ewhitmore",
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(7200))
            };

            // Act
            var newToken = SecurityService.AddRefreshToken(token);

            // Assert
            Assert.AreEqual(token.ReferenceId, newToken.ReferenceId);

        }

        [TestMethod]
        public void SecurityService_RemoveRefreshTokenById_ReturnTrue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var results = SecurityService.RemoveRefreshToken("qsKugCAf4/uPCkmZrQIkFAjwFUUk2aDoToGyuGqpdNQ=");

            // Assert
            Assert.IsTrue(results);
        }

        [TestMethod]
        public void SecurityService_RemoveRefreshTokenById_ReturnFalse()
        {
            // Arrange
            SeedDatabase();

            // Act
            var results = SecurityService.RemoveRefreshToken("112233");

            // Assert
            Assert.IsFalse(results);
        }

        [TestMethod]
        public void SecurityService_RemoveRefreshToken_ReturnNull()
        {
            // Arrange
            SeedDatabase();

            // Act
            var token = RefreshTokenDao.GetAll().First();
            var id = token.Id;
            SecurityService.RemoveRefreshToken(token);
            var dbToken = RefreshTokenDao.Get(id);

            // Assert
            Assert.IsNull(dbToken);

        }

        [TestMethod]
        public void SecurityService_RemoveRefreshToken_NoException()
        {
            // Arrange
            SeedDatabase();

            // Act
            SecurityService.RemoveRefreshToken(new RefreshToken());

            // Assert
            // Should not throw exception
        }

        [TestMethod]
        public void SecurityService_FindRefreshToken_ReturnTrue()
        {
            // Arrange
            SeedDatabase();
            const string tokenSubject = "new_user";

            // Act
            var token = SecurityService.FindRefreshToken("qsKugCAf4/12312312312312=");

            // Assert
            Assert.AreEqual(tokenSubject,token.Subject);
        }

        [TestMethod]
        public void SecurityService_FindRefreshToken_ReturnNull()
        {
            // Arrange
            SeedDatabase();
            const string tokenSubject = "new_user";

            // Act
            var token = SecurityService.FindRefreshToken("NOTATREF");

            // Assert
            Assert.IsNull(token);
        }

        [TestMethod]
        public void SecurityService_GetAllRefreshTokens_ReturnTrue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var tokens = SecurityService.GetAllRefreshTokens();

            // Assert
            Assert.AreEqual(6,tokens.Count);
        }
    }
}
