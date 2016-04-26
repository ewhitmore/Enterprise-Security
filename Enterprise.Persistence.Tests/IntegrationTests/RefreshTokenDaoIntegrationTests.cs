using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Enterprise.Model;
using Enterprise.Persistence.Dao;
using Enterprise.Web;
using FluentNHibernate.Testing.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Context;

namespace Enterprise.Persistence.Tests.IntegrationTests
{
    [TestClass]
    public class RefreshTokenDaoIntegrationTests
    {
        public IRefreshTokenDao RefreshTokenDao { get; set; }

        [TestInitialize]
        public void Init()
        {
            TestingSessionFactory.CreateSessionFactory("thread_static"); // Create a thread_static NHibernate Session
            AutofacConfig.RegisterAutofac(TestingSessionFactory.SessionFactory); // Send session to IoC Container
            CurrentSessionContext.Bind(TestingSessionFactory.Session); // Bind to Unit Test Context
            RefreshTokenDao = AutofacConfig.Container.Resolve<IRefreshTokenDao>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            HibernateConfig.Dispose();  // Release Session
        }

        [TestMethod]
        public void RefreshTokenDao_SaveRefreshToken_ReturnsTrue()
        {
            // Arrange
            var token = new RefreshToken()
            {
                ClientId = "ngAuthApp",
                ReferenceId = "qsKugCAf4/uPCkmZrQIkFAjwFUUk2aDoToGyuGqpdNQ=",
                IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk"

            };

            // Act
            RefreshTokenDao.Save(token);

            // Assert
            Assert.AreEqual(1, token.Id);
        }

        [TestMethod]
        public void RefreshTokenDao_GetRefreshTokenById_ReturnsTrue()
        {
            // Arrange
            var token = new RefreshToken()
            {
                ClientId = "ngAuthApp",
                ReferenceId = "qsKugCAf4/uPCkmZrQIkFAjwFUUk2aDoToGyuGqpdNQ=",
                IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk"

            };

            RefreshTokenDao.Save(token);

            // Act
            var dbToken = RefreshTokenDao.Get(1);

            // Assert
            Assert.AreEqual(1, dbToken.Id);
        }

        [TestMethod]
        public void RefreshTokenDao_GetAllRefreshTokens_Returns5()
        {
            // Arrange
            SeedDatabase();

            // Act
            var tokens = RefreshTokenDao.GetAll();

            // Assert
            Assert.AreEqual(6, tokens.Count);
        }

        [TestMethod]
        public void RefreshTokenDao_FindRefreshToken_ReturnsTrue()
        {
            // Arrange
            const string referenceId = "qsKugCAf4/12222111=";
            SeedDatabase();

            // Act
            var token = RefreshTokenDao.FindAll().First(t => t.ReferenceId == referenceId);

            // Assert
            Assert.AreEqual(token.ReferenceId, referenceId);
        }

        [TestMethod]
        public void RefreshTokenDao_UpdateRefId_ReturnsTeacher()
        {
            // Arrange
            SeedDatabase();
            const string referenceId = "123456789123456789aaabbbccccc";
            var token = RefreshTokenDao.FindAll().First(t => t.ReferenceId == "qsKugCAf4/12222111=");

            // Act
            token.ReferenceId = referenceId;
            RefreshTokenDao.Update(token);
            RefreshTokenDao.Flush();

            var dbToken = RefreshTokenDao.FindAll().First(x => x.ReferenceId == referenceId);

            // Assert
            Assert.AreEqual(referenceId, token.ReferenceId); // Test cache object
            Assert.AreEqual(referenceId, dbToken.ReferenceId); // Test database object
        }

        [TestMethod]
        public void RefreshTokenDao_DeleteRefreshToken_Returns0()
        {

            // Arrange
            SeedDatabase();
            var token = RefreshTokenDao.FindAll().First(t => t.ReferenceId == "qsKugCAf4/12222111=");

            // Act
            RefreshTokenDao.Delete(token);
            RefreshTokenDao.Flush();

            var tokens = RefreshTokenDao.FindAll().Where(t => t.ReferenceId == "qsKugCAf4/12222111=");

            // Assert
            Assert.AreEqual(0,tokens.Count());
        }

        private void SeedDatabase()
        {
            var tokens = new List<RefreshToken>()
            {
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/uPCkmZrQIkFAjwFUUk2aDoToGyuGqpdNQ=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/uPCkmZrQIkFAjwFUUk2aDoToGyuGqpdNQ=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-7890789-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/12312312312312=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-79078907890-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/12222111=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62G456456456hhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/gfhjfndb b=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk331237qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk"

                },
                new RefreshToken()
                {
                    ClientId = "ngAuthApp",
                    ReferenceId = "qsKugCAf4/asdfasdf=",
                    IssuedUtc = new DateTime(2016, 04, 01, 15, 29, 25, 000),
                    ExpiresUtc = new DateTime(2016, 06, 01, 15, 29, 25, 000),
                    ProtectedTicket = "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-sdfgsdfgs-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk"

                }
            };

            foreach (var refreshToken in tokens)
            {
                RefreshTokenDao.Save(refreshToken);
            }
        }

    }
}
