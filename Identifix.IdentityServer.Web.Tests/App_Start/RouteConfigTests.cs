using System;
using System.Linq;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identifix.IdentityServer.Tests
{
    [TestClass]
    public class RouteConfigTests
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestCleanup]
        public void CleanUp()
        {
            
        }

        [TestMethod]
        public void RegisterRoutes_WithNullRouteCollection_ThrowsGuardException()
        {
            AssertThrows<GuardException>(() => RouteConfig.RegisterRoutes(null));
        }

        [TestMethod]
        public void RegisterRoutes_WithValidRouteCollection_AddsDefaultRoute()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            Assert.IsTrue(routes.OfType<Route>().Any(route => route.Url == "{controller}/{action}/{id}"));
        }

        [TestMethod]
        public void RegisterRoutes_WithValidRouetsCollection_AddsIgnoreRouteForAxd()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            Assert.IsTrue(routes.OfType<Route>().Any(route => route.Url == "{resource}.axd/{*pathInfo}"));
        }

        public TException AssertThrows<TException>(Action action) where TException : Exception
        {
            TException exception = null;
            try
            {
                action.Invoke();
            }
            catch (TException tex)
            {
                exception = tex;
            }
            Assert.IsNotNull(exception);
            return exception;
        }
    }
}