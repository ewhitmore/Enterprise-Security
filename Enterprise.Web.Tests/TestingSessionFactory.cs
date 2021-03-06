﻿using Enterprise.Persistence.Dao.Mapping;
using Enterprise.Persistence.Model;
using Enterprise.Persistence.Tests;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.AspNet.Identity.Helpers;
using NHibernate.Tool.hbm2ddl;

namespace Enterprise.Web.Tests
{
    internal class TestingSessionFactory
    {
        public static ISessionFactory SessionFactory { get; set; }
        public static ISession Session { get; set; }

        public static void CreateSessionFactory(string sessionContext)
        {
            var config = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .InMemory()
                    //.ConnectionString("Data Source=:memory:;Version=3;New=True")
                    .Dialect<CustomDialect>()
                    .Driver<NHibernate.Driver.SQLite20Driver>()
                    .ShowSql()
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<StudentMap>())

                .ExposeConfiguration(cfg => { cfg.AddDeserializedMapping(MappingHelper.GetIdentityMappings(new[] { typeof(ApplicationUser) }), null); })

                .ExposeConfiguration(cfg => cfg.SetProperty("current_session_context_class", sessionContext))
                .ExposeConfiguration(cfg => cfg.SetProperty("adonet.batch_size", "100"))
                .ExposeConfiguration(cfg => cfg.SetProperty("connection.release_mode", "on_close")) // Required for unit testing
                .BuildConfiguration();

            SessionFactory = config.BuildSessionFactory();

            SessionFactory = SessionFactory;
            Session = SessionFactory.OpenSession();
            new SchemaExport(config).Execute(true, true, false, Session.Connection, null);

        }

    }
}
