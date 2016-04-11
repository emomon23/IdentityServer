using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identifix.IdentityServer.Tests
{
    [TestClass]
    public class WebApiConfigTests
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
        public void Register_WithNullConfiguration_ThrowsGuardException()
        {
            AssertThrows<GuardException>(() => WebApiConfig.Register(null));
        }

        [TestMethod]
        public void Register_WithValidConfiguration_AddsDefaultApiRoute()
        {
            HttpConfiguration configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);

            Assert.IsTrue(configuration.Routes.OfType<HttpRoute>().Any(route => route.RouteTemplate == "api/{controller}/{action}/{id}"));
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