using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Enterprise.Persistence;
using Enterprise.Persistence.Dao;
using Enterprise.Persistence.Dao.Implementation;
using Enterprise.Web.Services;
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

            // Either use a session in view model or per instance depending on the context.
            if (HttpContext.Current != null)
            {
                // Indicates a web based implementation
                builder.RegisterInstance(HibernateConfig.CreateSessionFactory("SampleDatabase", "web"));
                builder.Register(s => s.Resolve<ISessionFactory>().OpenSession()).InstancePerRequest();

            }

            // Add Peristance Configuration
            RegisterPersistanceLayer(builder);

            // Add Services
            AddServices(builder);

            // Add Types
            AddTypes(builder);

            // Set the dependency resolver to be Autofac.
            Container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);

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
        }

        private static void AddServices(ContainerBuilder builder)
        {
            var serviceAssembly = Assembly.GetAssembly(typeof(StudentService));

            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerRequest();
        }
    }
}