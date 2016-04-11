using Identifix.IdentityServer.Infrastructure;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Identifix.IdentityServer.Tests.Infrastructure
{
    [TestClass]
    public class WebApplicationContextTests
    {
        private ISettingManager _mockSettings;
        private IStateManager _mockState;
        private ILog _logger;

        [TestInitialize]
        public void Initialize()
        {
            _mockSettings = MockRepository.GenerateMock<ISettingManager>();
            _mockState = MockRepository.GenerateMock<IStateManager>();
            _logger = MockRepository.GenerateMock<ILog>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mockSettings = null;
            _mockState = null;
            _logger = null;
        }

        [TestMethod]
        public void Instantiation_WithNullSettings_ThrowsGuardException()
        {
            CustomAssert.Throws<GuardException>(() => CreateSut(null, _mockState, _logger));
        }

        [TestMethod]
        public void Instantiation_WithNullState_ThrowsGuardException()
        {
            CustomAssert.Throws<GuardException>(() => CreateSut(_mockSettings, null, _logger));
        }

        [TestMethod]
        public void Instantiation_WithNullLogger_ThrowsGuardException()
        {
            CustomAssert.Throws<GuardException>(() => CreateSut(_mockSettings, _mockState, null));
        }

        [TestMethod]
        public void Instantiation_WithValidArguments_InitializesState()
        {
            WebApplicationContext sut = CreateSut();

            Assert.IsNotNull(sut.State);
            Assert.AreSame(_mockState, sut.State);
        }

        [TestMethod]
        public void Instantiation_WithValidArguments_InitializesSettings()
        {
            WebApplicationContext sut = CreateSut();

            Assert.IsNotNull(sut.Settings);
            Assert.AreSame(_mockSettings, sut.Settings);
        }

        [TestMethod]
        public void Instantiation_WithValidArguments_InitializesLogger()
        {
            WebApplicationContext sut = CreateSut();

            Assert.IsNotNull(sut.Logger);
            Assert.AreSame(_logger, sut.Logger);
        }

        public WebApplicationContext CreateSut()
        {
            return CreateSut(_mockSettings, _mockState, _logger);
        }

        public WebApplicationContext CreateSut(ISettingManager settings, IStateManager state, ILog logger)
        {
            return new WebApplicationContext(settings, state, logger);
        }
    }
}