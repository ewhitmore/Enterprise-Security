using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Enterprise.Model;
using Enterprise.Persistence.Dao.Mapping;

using Enterprise.Persistence.Model;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.AspNet.Identity;
using NHibernate.AspNet.Identity.Helpers;
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
        /// <see href="http://nhibernate.info/doc/nhibernate-reference/architecture.html#architecture-current-session">here</see></param>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory(string connectionStringName, string context)
        {



            var list = new List<Type>();
            list.Add(typeof(ApplicationUser));

            //string @namespace = "Enterprise.Persistence.Dao.Mapping";

            //var q = from t in Assembly.GetExecutingAssembly().GetTypes()
            //        where t.IsClass && t.Namespace == @namespace
            //        select t;
            //q.ToList().ForEach(t => Debug.WriteLine(t.Name));

            //list.AddRange(q);

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

                .ExposeConfiguration(cfg => { cfg.AddDeserializedMapping(MappingHelper.GetIdentityMappings(list.ToArray()), null); })

                // Comment out CreateSchema to keep nhibernate from removing the data each time.
                
                
                .ExposeConfiguration(cfg => cfg.SetProperty("current_session_context_class", context))
                .ExposeConfiguration(cfg => cfg.SetProperty("adonet.batch_size", "100"))
                .ExposeConfiguration(cfg => cfg.SetProperty("query.substitutions", "true 1, false 0"))
                //.ExposeConfiguration(cfg => cfg.SetProperty("use_proxy_validator", "true"))
                .ExposeConfiguration(SchemaSelector)
                .BuildConfiguration()
                .BuildSessionFactory();

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
