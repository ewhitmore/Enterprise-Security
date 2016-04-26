using System;
using Enterprise.Model;
using Enterprise.Web;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Context;

namespace Enterprise.Persistence.Tests.UnitTests
{
    [TestClass]
    public class MappingOwinUnitTests
    {
        // Nhibernate Mapping Unit Testing
        // https://github.com/jagregory/fluent-nhibernate/wiki/Persistence-specification-testing

        public ISession Session { get; set; }

        [TestInitialize]
        public void Init()
        {
            TestingSessionFactory.CreateSessionFactory("thread_static"); // Create a thread_static NHibernate Session
            AutofacConfig.RegisterAutofac(TestingSessionFactory.SessionFactory); // Send session to IoC Container
            CurrentSessionContext.Bind(TestingSessionFactory.Session); // Bind to Unit Test Context

            Session = HibernateConfig.SessionFactory.GetCurrentSession();
        }

        [TestCleanup]
        public void CleanUp()
        {
            HibernateConfig.Dispose();  // Release Session

        }

        [TestMethod]
        public void Mapping_CorrectlyMapClient_ReturnsTrue()
        {
            new PersistenceSpecification<Client>(Session)
                .CheckProperty(x => x.Id, 1)
                .CheckProperty(x => x.ClientId, "ngAuthApp")
                .CheckProperty(x => x.Active, true)
                .CheckProperty(x => x.AllowedOrigin, "*")
                .CheckProperty(x => x.Name,"AngularJS front-end Application" )
                .CheckProperty(x => x.RefreshTokenLifeTime, 7200)
                .CheckProperty(x => x.Secret, "6YV7M1r981yoGhELyB84aC+KiYksxZf1OY3++C1CtRN=")

                .VerifyTheMappings();
        }

        [TestMethod]
        public void Mapping_CorrectlyMapRefreshToken_ReturnsTrue()
        {
            new PersistenceSpecification<RefreshToken>(Session)
                .CheckProperty(x => x.Id, 1)
                .CheckProperty(x => x.ReferenceId, "qsKugCAf4/uPCkmZrQIkFAjwFUUk2aDoToGyuGqpdNQ=")
                .CheckProperty(x => x.ClientId, "ngAuthApp")
                .CheckProperty(x => x.Subject, "USERNAME")
                .CheckProperty(x => x.IssuedUtc, new DateTime(2016,05,01,15,29,25,000))
                .CheckProperty(x => x.ExpiresUtc, new DateTime(2016, 05, 01, 15, 29, 25, 000))
                .CheckProperty(x => x.ProtectedTicket, "xcRxTKEdbvi4nk7qjBmduUm5hmNKNNQ-RubBW_hbARxV9eOU62GhhzcZ1IpuwN6-5R7GCl7kbiKCXj-bhswV21HpnI8iQfyW2151PmJ86900KvYzCWTtRJDfiKTeCMY7i-EUfPh9yd07hYm0wnpEJirbXTF4-LRhtpjzOJIk-TPGdLy-6yoHMI7l3dlaRKPTu_IaCQk8ZJeMfIsTYgqyJ0r3f7qzehObfaH_nSXKOd6BY4eYjMSU9i_s_LRk6sI2c2yxZcjKzGSo4JMPtVMNAVlTwE8kBb811jrhzrV9wYx4WtViKlUPVfZC2fyI2CwGirHJhAsvwoGIoDyPwjoL6Z906E3OkhGrusncyMvvSUAly1MLfRD3dewRMb5_EdGk")

                .VerifyTheMappings();
        }
    }
}
