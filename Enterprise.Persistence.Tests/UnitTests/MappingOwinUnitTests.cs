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
            //new PersistenceSpecification<Client>(Session)
            //    .CheckProperty(x => x.Id, 1)
            //    .CheckProperty(x => x.ClientId, "ngAuthApp")
            //    .CheckProperty(x => x.Active, true)
            //    .CheckProperty(x => x.AllowedOrigin, "*")
            //    .CheckProperty(x => x.Name, "AngularJS front-end Application")
            //    .CheckProperty(x => x.RefreshTokenLifeTime, 7200)
            //    .CheckProperty(x => x.Secret, "6YV7M1r981yoGhELyB84aC+KiYksxZf1OY3++C1CtRN=")

            //    .VerifyTheMappings();
        }
    }
}
