﻿using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Enterprise.Persistence;
using Enterprise.Persistence.Dao;
using Enterprise.Persistence.Dao.Implementation;
using Enterprise.Web.Security;
using Enterprise.Web.Services;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using NHibernate;

namespace Enterprise.Web
{
    public static class AutofacConfig
    {
        public static IContainer Container { get; set; }

        public static void RegisterAutofac()
        {

            var builder = new ContainerBuilder();

            // Register Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();


            // Indicates a web based implementation
            builder.RegisterInstance(HibernateConfig.CreateSessionFactory("EnterpriseSecurityDatabase", "web"));
            builder.Register(s => s.Resolve<ISessionFactory>().OpenSession()).InstancePerRequest();

            // Add Peristance Configuration
            RegisterPersistanceLayer(builder);

            // Add Services
            AddServices(builder);

            // Add Types
            AddTypes(builder);

            // Other
            AddOther(builder);

            // Set the dependency resolver to be Autofac.
            Container = builder.Build();
        }

        public static void RegisterAutofac(ISessionFactory sessionFactory)
        {
            var builder = new ContainerBuilder();

            // Register Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            // Either use a session in view model or per instance depending on the context.
            builder.RegisterInstance(HibernateConfig.CreateSessionFactory(sessionFactory));
            builder.Register(s => s.Resolve<ISessionFactory>().GetCurrentSession()).InstancePerLifetimeScope();


            // Add Peristance Configuration
            RegisterPersistanceLayer(builder);

            // Add Services
            AddServices(builder);

            // Add Types
            AddTypes(builder);

            // Other
            AddOther(builder);

            // Set the dependency resolver to be Autofac.
            Container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);

        }

        private static void RegisterPersistanceLayer(ContainerBuilder builder)
        {
            // Add Repository
            builder.RegisterGeneric(typeof(Repository<,>))
                .As(typeof(IRepository<,>))
                .PropertiesAutowired()
                .InstancePerRequest();
        }

        private static void AddTypes(ContainerBuilder builder)
        {
            builder.RegisterType<TeacherDao>().As<ITeacherDao>().PropertiesAutowired();
            builder.RegisterType<StudentDao>().As<IStudentDao>().PropertiesAutowired();
            builder.RegisterType<ClassroomDao>().As<IClassroomDao>().PropertiesAutowired();
            builder.RegisterType<ClientDao>().As<IClientDao>().PropertiesAutowired();
            builder.RegisterType<RefreshTokenDao>().As<IRefreshTokenDao>().PropertiesAutowired();
        }

        private static void AddServices(ContainerBuilder builder)
        {
            var serviceAssembly = Assembly.GetAssembly(typeof(StudentService));

            // Set HTTP Contexts to InstancePerRequest
            if (HttpContext.Current != null)
            {
                builder.RegisterAssemblyTypes(serviceAssembly)
                 .Where(t => t.Name.EndsWith("Service"))
                 .AsImplementedInterfaces()
                 .PropertiesAutowired()
                 .InstancePerRequest();

            }
            else
            {

                builder.RegisterAssemblyTypes(serviceAssembly)
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .PropertiesAutowired()
                    .InstancePerLifetimeScope();

            }
        }

        private static void AddOther(ContainerBuilder builder)
        {
            //http://stackoverflow.com/questions/25871392/autofac-dependency-injection-in-implementation-of-oauthauthorizationserverprovid

            builder
                .RegisterType<SimpleAuthorizationServerProvider>()
                .As<IOAuthAuthorizationServerProvider>()
                //.PropertiesAutowired()
                .SingleInstance()
                ; // you only need one instance of this provider


            builder
                .RegisterType<SimpleRefreshTokenProvider>()
                .As<IAuthenticationTokenProvider>()
                //.PropertiesAutowired()
                .SingleInstance();  // you only need one instance of this provider
        }
    }
}