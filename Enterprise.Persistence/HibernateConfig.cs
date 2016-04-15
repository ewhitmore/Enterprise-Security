using System.Configuration;
using Enterprise.Persistence.Dao.Mapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

namespace Enterprise.Persistence
{
    public class HibernateConfig
    {
        public static ISessionFactory SessionFactory { get; private set; }



        /// <summary>
        /// Creates a MsSql2012 session factory from connection string name
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="context">Context Can be call, thread_static or web. More information can be found 
        /// <see href="http://nhibernate.info/doc/nhibernate-reference/architecture.html#architecture-current-session">here</see></param></param>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory(string connectionStringName, string context)
        {
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration
                    .MsSql2012
                    .ConnectionString(c => c.FromConnectionStringWithKey(connectionStringName)
                    )
                .ShowSql()
                // Turn this on for production only
                //.UseReflectionOptimizer
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<StudentMap>())
                //.Mappings(m => m.FluentMappings.Add<LogMap>())

                // Comment out CreateSchema to keep nhibernate from removing the data each time.
                .ExposeConfiguration(SchemaSelector)
                //.ExposeConfiguration(cfg => cfg.SetInterceptor(new SqlStatementInterceptor()))
                .ExposeConfiguration(cfg => cfg.SetProperty("current_session_context_class", context))
                .ExposeConfiguration(cfg => cfg.SetProperty("adonet.batch_size", "100"))
                .BuildConfiguration().BuildSessionFactory();

            SessionFactory = sessionFactory;

            return sessionFactory;
        }

        /// <summary>
        /// Creates a session factory from a fully configured session factory
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;

            return sessionFactory;
        }

        private static void SchemaSelector(Configuration cfg)
        {
            switch (ConfigurationManager.AppSettings["Schema"])
            {
                case "CREATE":
                    var schemaExport = new SchemaExport(cfg);
                    schemaExport.Drop(false, true);
                    schemaExport.Create(false, true);
                    break;
                case "UPDATE":
                    var schemaUpdate = new SchemaUpdate(cfg);
                    schemaUpdate.Execute(false, true);
                    break;
                default:
                    var schemaValidator = new SchemaValidator(cfg);
                    schemaValidator.Validate();
                    break;
            }
        }

        public static void Dispose()
        {
            SessionFactory.Dispose();
        }
    }
}
