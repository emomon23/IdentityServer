using Identifix.IdentityServer.Controllers;
using Identifix.IdentityServer.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Identifix.IdentityServer.Tests.Controllers
{
    [TestClass]
    public class MvcControllerBaseTests
    {
        private ControllerMocks _mocks;

        [TestInitialize]
        public void Initialize()
        {
            _mocks = new ControllerMocks();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mocks = null;
        }

        [TestMethod]
        public void Instantiation_WithNullContext_ThrowsGuardException()
        {
            CustomAssert.Throws<GuardException>(() => CreateSut(null));
        }

        [TestMethod]
        public void Instantiation_WithValidContext_InitializesState()
        {
            MvcControllerBase sut = CreateSut();

            Assert.IsNotNull(sut.State);
            Assert.AreSame(_mocks.State, sut.State);
        }

        [TestMethod]
        public void Instantiation_WithValidContext_InitializesSettings()
        {
            MvcControllerBase sut = CreateSut();

            Assert.IsNotNull(sut.Settings);
            Assert.AreSame(_mocks.Settings, sut.Settings);
        }

        private MvcControllerBase CreateSut()
        {
            return CreateSut(_mocks.Context);
        }

        private MvcControllerBase CreateSut(IApplicationContext context)
        {
            return new MvcControllerBaseTestController(context);
        }
    }
}