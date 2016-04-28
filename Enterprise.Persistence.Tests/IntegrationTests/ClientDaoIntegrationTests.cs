using System.Collections.Generic;
using System.Linq;
using Autofac;
using Enterprise.Model;
using Enterprise.Persistence.Dao;
using Enterprise.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Context;

namespace Enterprise.Persistence.Tests.IntegrationTests
{
    [TestClass]
    public class ClientDaoIntegrationTests
    {
        public IClientDao ClientDao { get; set; }

        [TestInitialize]
        public void Init()
        {
            TestingSessionFactory.CreateSessionFactory("thread_static"); // Create a thread_static NHibernate Session
            AutofacConfig.RegisterAutofac(TestingSessionFactory.SessionFactory); // Send session to IoC Container
            CurrentSessionContext.Bind(TestingSessionFactory.Session); // Bind to Unit Test Context
            ClientDao = AutofacConfig.Container.Resolve<IClientDao>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            HibernateConfig.Dispose();  // Release Session
        }

        [TestMethod]
        public void ClientDao_SaveClient_ReturnsTrue()
        {
            // Arrange
            var client = new Client()
            {
                ClientId = "AngularWebClient",
                Active = true,
                AllowedOrigin = "*",
                ApplicationType = ApplicationTypes.JavaScript,
                Name = "AngularJS front-end Application",
                RefreshTokenLifeTime = 7200,
                Secret = "5YV7M1r981yoGhELyB84aC+KiYksxZf1OY3++C1CtRM="
            };

            // Act
            ClientDao.Save(client);

            // Assert
            Assert.AreEqual(1, client.Id);
        }

        [TestMethod]
        public void ClientDao_GetAllClient_ReturnsTrue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var clients = ClientDao.GetAll();

            // Assert
            Assert.AreEqual(3, clients.Count);
        }

        [TestMethod]
        public void ClientDao_FindClient_ReturnsTrue()
        {
            // Arrange
            SeedDatabase();

            // Act
            var client = ClientDao.FindAll().First(x => x.Id == 1);

            // Assert
            Assert.AreEqual(1,client.Id);
        }

        [TestMethod]
        public void ClientDao_UpdateActive_ReturnsTrue()
        {
            // Arrange
            SeedDatabase();
            const string secret = "5YV7M1r981yoGhELyB84aC+444556++C1CtRM=NEW";
            var client = ClientDao.FindAll().First(c => c.Secret == "5YV7M1r981yoGhELyB84aC+444556++C1CtRM=");


            // Act
            client.Secret = secret;
            ClientDao.Update(client);
            ClientDao.Flush();

            var dbClient = ClientDao.FindAll().First(x => x.Secret == secret);

            // Assert
            Assert.AreEqual(secret, client.Secret); // Test cache object
            Assert.AreEqual(secret, dbClient.Secret); // Test database object
        }

        [TestMethod]
        public void ClientDao_DeleteActive_ReturnsTrue()
        {
            // Arrange
            SeedDatabase();
            var client = ClientDao.FindAll().First(x => x.Id == 1);


            // Act
            ClientDao.Delete(client);
            ClientDao.Flush();

            var clients = ClientDao.GetAll().Where(x => x.Id == 1);

            // Assert
            Assert.AreEqual(0, clients.Count());


        }

        private void SeedDatabase()
        {
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

        }
    }
}
