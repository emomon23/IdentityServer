using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json.Serialization;

namespace Identifix.IdentityServer
{
    public class MvcApplication : HttpApplication
    {
        [ExcludeFromCodeCoverage]
        protected void Application_Start()
        {
            DependencyConfig.RegisterDependencyResolver();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration
            .Formatters
            .JsonFormatter
            .SerializerSettings
            .ContractResolver = new CamelCasePropertyNamesContractResolver();

        }
    }
}
