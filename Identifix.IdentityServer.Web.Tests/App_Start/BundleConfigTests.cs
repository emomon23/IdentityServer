using System;
using System.CodeDom;
using System.Linq;
using System.Web.Optimization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identifix.IdentityServer.Tests
{
    [TestClass]
    public class BundleConfigTests
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
        public void RegisterBundles_WithNullBundles_ThrowsGuardException()
        {
            CustomAssert.Throws<GuardException>(() => BundleConfig.RegisterBundles(null));
        }

        [TestMethod]
        public void RegisterBundles_WithValidBundles_AddsJQueryScriptBundle()
        {
            const string expectedPath = "~/bundles/jquery";
            BundleCollection bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);

            ScriptBundle expected = bundles.OfType<ScriptBundle>().SingleOrDefault(bundle => bundle.Path == expectedPath);
            Assert.IsNotNull(expected);
        }

        [TestMethod]
        public void RegisterBundles_WithValidBundles_AddsJQueryValidationScriptBundle()
        {
            const string expectedPath = "~/bundles/jqueryval";
            BundleCollection bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);

            ScriptBundle expected = bundles.OfType<ScriptBundle>().SingleOrDefault(bundle => bundle.Path == expectedPath);
            Assert.IsNotNull(expected);
        }
        
        [TestMethod]
        public void RegisterBundles_WithValidBundles_AddsModernizrScriptBundle()
        {
            const string expectedPath = "~/bundles/modernizr";
            BundleCollection bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);

            ScriptBundle expected = bundles.OfType<ScriptBundle>().SingleOrDefault(bundle => bundle.Path == expectedPath);
            Assert.IsNotNull(expected);
        }

        [TestMethod]
        public void RegisterBundles_WithValidBundles_AddsBootstrapScriptBundle()
        {
            const string expectedPath = "~/bundles/bootstrap";
            BundleCollection bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);

            ScriptBundle expected = bundles.OfType<ScriptBundle>().SingleOrDefault(bundle => bundle.Path == expectedPath);
            Assert.IsNotNull(expected);
        }
        
        [TestMethod]
        public void RegisterBundles_WithValidBundles_AddsStyleBundle()
        {
            const string expectedPath = "~/Content/css";
            BundleCollection bundles = new BundleCollection();
            BundleConfig.RegisterBundles(bundles);

            StyleBundle expected = bundles.OfType<StyleBundle>().SingleOrDefault(bundle => bundle.Path == expectedPath);
            Assert.IsNotNull(expected);
        }
    }
}