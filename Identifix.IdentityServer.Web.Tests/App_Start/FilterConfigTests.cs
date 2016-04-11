using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identifix.IdentityServer.Tests
{
    [TestClass]
    public class FilterConfigTests
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestCleanup]
        public void CleanUp()
        {
            
        }

        // Tests
        [TestMethod]
        public void RegisterGlobalFilters_WithNullFilters_ThrowsGuardException()
        {
            AssertThrows<GuardException>(() => FilterConfig.RegisterGlobalFilters(null));
        }

        [TestMethod]
        public void RegisterGlobalFilters_WithValidFilters_AddsHandleErrorAttributeToFilters()
        {
            GlobalFilterCollection filters = new GlobalFilterCollection();
            FilterConfig.RegisterGlobalFilters(filters);

            Assert.IsTrue(filters.Any(filter => filter.Instance is HandleErrorAttribute));
        }

        // Helpers
        public FilterConfig CreateSut()
        {
            return new FilterConfig();
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