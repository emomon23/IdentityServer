using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Identifix.IdentityServer.Infrastructure;
using Identifix.IdentityServer.Models.Data;
using log4net;

namespace Identifix.IdentityServer
{
    [ExcludeFromCodeCoverage]
    public static class DependencyConfig
    {
        private static IContainer instance;

        public static IContainer Instance
        {
            get { return instance ?? (instance = CreateContainer()); } 
        }

        private static IContainer CreateContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            ConfigureDependencyMappingDefaults(builder);
            ConfigureDependencyMappingOverrides(builder);
            return builder.Build();
        }

        public static IContainer RegisterDependencyResolver()
        {
           
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Instance));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Instance);
            return Instance;
        }

        private static void ConfigureDependencyMappingDefaults(ContainerBuilder builder)
        {
            // Load Assemblies
            Assembly webAssembly = Assembly.GetAssembly(typeof(MvcApplication));
            Assembly coreAssembly = Assembly.GetAssembly(typeof (IApplicationContext));
            Assembly dataAssembly = Assembly.GetAssembly(typeof(SqlContext));
            builder.RegisterControllers(webAssembly);
            builder.RegisterApiControllers(webAssembly);

            // Assembly Type Registration
            builder.RegisterAssemblyTypes(webAssembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(coreAssembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(dataAssembly).AsImplementedInterfaces().InstancePerLifetimeScope();

            // Custom Type Mappings
            builder.RegisterType<SqlContext>().AsSelf();

            // Logging
            log4net.Config.XmlConfigurator.Configure();
            var logger = LogManager.GetLogger("Identifix SSO - Log Manager");
            builder.Register(l => logger).As<ILog>();


            // MVC Specific Mappings
            builder.RegisterModule(new AutofacWebTypesModule());
        }

        private static void ConfigureDependencyMappingOverrides(ContainerBuilder builder)
        {

        }
    }
}