using System.Data;
using Enterprise.Persistence.Dao.Mapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using NHibernate.Tool.hbm2ddl;

namespace Enterprise.Persistence.Tests
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


    /// <summary>
    /// Extend SQLiteDialect to convert DATETIME2 to TEXT which is supported by SQLITE
    /// http://ewhitmor.blogspot.com/2016/04/sqlite-nhibernate-datetime2.html
    /// </summary>
    public class CustomDialect : SQLiteDialect
    {
        protected override void RegisterColumnTypes()
        {
            base.RegisterColumnTypes();
            RegisterColumnType(DbType.DateTime2, "DATETIME2");
        }

        protected override void RegisterFunctions()
        {
            base.RegisterFunctions();
            RegisterFunction("current_timestamp", new NoArgSQLFunction("TEXT", NHibernateUtil.DateTime2, true));

        }

        protected override void RegisterKeywords()
        {
            base.RegisterKeywords();
            RegisterKeyword("datetime2");

        }

        protected override void RegisterDefaultProperties()
        {
            base.RegisterDefaultProperties();

        }
    }

}
