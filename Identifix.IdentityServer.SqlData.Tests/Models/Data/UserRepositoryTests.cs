using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using Identifix.IdentityServer.Infrastructure;
using Identifix.IdentityServer.Models;
using Identifix.IdentityServer.Models.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Identifix.IdentityServer.Tests.Models.Data
{
    [TestClass]
    public class UserRepositoryTests
    {
        private const int ExpectedCountryCount = 249;
        private const int ExpectedUnitedStatesStateCount = 62;
        private const int UnitedStatesCountryId = 1;
        private const string UnitedStatesCode = "US";
        private const string ArgentinaCode = "AR";
        private const string BadCountryCode = "ZZ";
        private const int BadCountryId = int.MaxValue;
        private const int ArgentinaCountryId = 12;
        private readonly string _testConnectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString;
        private IDatabaseSettings _mockSettings;
        private SqlContext _testContext;
        
        [TestInitialize]
        public void Initialize()
        {
            _mockSettings = MockRepository.GenerateMock<IDatabaseSettings>();
            _mockSettings.Stub(mock => mock.UserDatabaseConnectionString).Return(_testConnectionString);
            _testContext = new SqlContext(_mockSettings);
            TestDatabaseManager.InitializeDatabase();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _testContext = null;
            _mockSettings = null;
        }

        [TestMethod]
        public void RetrieveCountries_ReturnsAllCountries()
        {
            UserRepository sut = CreateSut();

            IList<Country> couuntries = sut.RetrieveCountries();

            Assert.AreEqual(ExpectedCountryCount, couuntries.Count);
        }

        [TestMethod]
        public void RetrieveStates_WithCountryCodeThatHasStates_ReturnsAllStates()
        {
            UserRepository sut = CreateSut();

            IList<State> states = sut.RetrieveStates(UnitedStatesCode);

            Assert.AreEqual(ExpectedUnitedStatesStateCount, states.Count);
        }
        
        [TestMethod]
        public void RetrieveStates_WithCountryCodeThatDoesNotHaveStates_ReturnsEmptyList()
        {
            UserRepository sut = CreateSut();

            IList<State> states = sut.RetrieveStates(ArgentinaCode);

            Assert.IsNotNull(states);
            Assert.AreEqual(0, states.Count);
        }

        [TestMethod]
        public void RetrieveStates_WithNonExistentCountryCode_ReturnsEmptyList()
        {
            UserRepository sut = CreateSut();

            IList<State> states = sut.RetrieveStates(BadCountryCode);

            Assert.IsNotNull(states);
            Assert.AreEqual(0, states.Count);
        }

        [TestMethod]
        public void RetrieveStates_WithValidCoutryIdThatHasStates_ReturnsAllStates()
        {
            UserRepository sut = CreateSut();

            IList<State> states = sut.RetrieveStates(UnitedStatesCountryId);

            Assert.AreEqual(ExpectedUnitedStatesStateCount, states.Count);
        }

        [TestMethod]
        public void RetrieveStates_WithValidCoutryIdThatHasNoStates_ReturnsEmptyList()
        {
            UserRepository sut = CreateSut();

            IList<State> states = sut.RetrieveStates(ArgentinaCountryId);

            Assert.IsNotNull(states);
            Assert.AreEqual(0, states.Count);
        }

        [TestMethod]
        public void RetrieveStates_WithNonExistentCoutryId_ReturnsEmptyList()
        {
            UserRepository sut = CreateSut();

            IList<State> states = sut.RetrieveStates(BadCountryId);

            Assert.IsNotNull(states);
            Assert.AreEqual(0, states.Count);
        }

        [TestMethod]
        public void CheckIfEmailExists_WhenUserWithEmailExists_ReturnsTrue()
        {
            const string email = "sly.juan@lpc.non";
            UserRepository sut = CreateSut();

            bool result = sut.CheckIfEmailExists(email);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfEmailExists_WhenNoUserWithEmailExists_ReturnsFalse()
        {
            const string email = "no.juan@lpc.non";
            UserRepository sut = CreateSut();

            bool result = sut.CheckIfEmailExists(email);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidToken_NullArguement_ThrowsException()
        {
            UserRepository sut = CreateSut();          

            CustomAssert.Throws<GuardException>(() => sut.IsValidToken(null));
        }

        [TestMethod]
        public void IsValidToken_WhitespaceArguement_ThrowsException()
        {
            UserRepository sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.IsValidToken("      "));
        }     

        public UserRepository CreateSut()
        {
            return CreateSut(_testContext);
        }

        private UserRepository CreateSut(SqlContext context)
        {
            return new UserRepository(context);
        }

    }
}