using System.Linq;
using Identifix.IdentityServer.Infrastructure;
using Identifix.IdentityServer.Models.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Identifix.IdentityServer.Tests.Models.Data
{
    [TestClass]
    public class SqlContextTests
    {
        private const string DummyConnectionString = "Server=.\\SQLExpress; Database=UserTestDatabase; Integrated Security = true;";
        private IDatabaseSettings _mockSettings;

        [TestInitialize]
        public void Initialize()
        {
            _mockSettings = MockRepository.GenerateMock<IDatabaseSettings>();
            _mockSettings.Stub(mock => mock.UserDatabaseConnectionString).Return(DummyConnectionString);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mockSettings = null;
        }

        [TestMethod]
        public void Instantiation_DisablesLazyLoading()
        {
            SqlContext sut = CreateSut();

            Assert.IsFalse(sut.Configuration.LazyLoadingEnabled);
        }

        public SqlContext CreateSut()
        {
            return CreateSut(_mockSettings);
        }

        public SqlContext CreateSut(IDatabaseSettings settings)
        {
            return new SqlContext(settings);
        }


    }
}