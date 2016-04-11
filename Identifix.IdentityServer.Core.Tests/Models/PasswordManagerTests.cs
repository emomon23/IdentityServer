using System;
using System.Text;
using Identifix.IdentityServer.Infrastructure;
using Identifix.IdentityServer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Identifix.IdentityServer.Tests.Models
{
    [TestClass]
    public class PasswordManagerTests
    {
        private ISecuritySettings _mockSettings;
        private const string TestPassword = "testP@55word";
        private const string TestPasswordHash = "1000:VGhpc0lzQVRlc3RTYWx0:LU2zBNzt/pIWRVonedMy1Arm3XZzpEftvvCSIfvRrQxureXkTLEi4D0cx+5Y2MMa8XjJA/2zz1K4I0vQ";
        private static readonly byte[] FixedSalt = Encoding.UTF8.GetBytes("ThisIsATestSalt");

        [TestInitialize]
        public void Initialize()
        {
            _mockSettings = MockRepository.GenerateMock<ISecuritySettings>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mockSettings = null;
        }

        [TestMethod]
        public void Instantiation_WithNullSettings_ThrowsGuardExcetpion()
        {
            CustomAssert.Throws<GuardException>(() => CreateSut(null));
        }

        [TestMethod]
        public void Iterations_WhenSettingsIsNotConfigured_ReturnsDefaultIterations()
        {
            _mockSettings.Stub(mock => mock.PasswordHashIterations).Return(0);
            PasswordManager sut = CreateSut();

            Assert.AreEqual(PasswordManager.DefaultIterations, sut.Iterations);
        }

        [TestMethod]
        public void HashPassword_WithNullPassword_ThrowsGuardException()
        {
            PasswordManager sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.HashPassword(null));
        }

        [TestMethod]
        public void HashPassword_WithEmptyPassword_ThrowsGuardException()
        {
            PasswordManager sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.HashPassword(string.Empty));
        }

        [TestMethod]
        public void HashPassword_WithWhiteSpacePassword_ThrowsGuardException()
        {
            PasswordManager sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.HashPassword("  \r\t"));
        }

        [TestMethod]
        public void HashPassword_WithValidPassword_ReturnsHash()
        {
            PasswordManager sut = CreateSut();            
            string hash = sut.HashPassword(TestPassword);
        }

        [TestMethod]
        public void HashPassword_WithValidPassword_ReturnsProperlyFormattedHash()
        {
            PasswordManager sut = CreateSut();

            string hash = sut.HashPassword(TestPassword);

            string[] parts = hash.Split(':');
            int output = 0;
            CustomAssert.DoesNotThrow(() => int.Parse(parts[0]));
            CustomAssert.DoesNotThrow(()=> Convert.FromBase64String(parts[1]));
            CustomAssert.DoesNotThrow(() => Convert.FromBase64String(parts[2]));
        }

        [TestMethod]
        public void HashPassword_WithValidPasswordAndFixedSalt_ReturnsExpectedHash()
        {
            PasswordManager sut = CreateSut();
            string result = sut.HashPassword(TestPassword, FixedSalt);
            
            Assert.AreEqual(TestPasswordHash, result);
        }

        [TestMethod]
        public void HashPassword_WithInValidPassword_ThrowsArguementException()
        {
            PasswordManager sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.HashPassword("invalid", FixedSalt));
        }

        [TestMethod]
        public void VerifyPassword_WithNullPassword_ThrowsGuardException()
        {
            PasswordManager sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.VerifyPassword(null, TestPasswordHash));
        }

        [TestMethod]
        public void VerifyPassword_WithEmptyPassword_ThrowsGuardException()
        {
            PasswordManager sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.VerifyPassword(string.Empty, TestPasswordHash));
        }

        [TestMethod]
        public void VerifyPassword_WithWhitespacePassword_ThrowsGuardException()
        {
            PasswordManager sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.VerifyPassword("  \t\r", TestPasswordHash));
        }

        [TestMethod]
        public void VerifyPassword_WithNullPasswordHash_ThrowsGuardException()
        {
            PasswordManager sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.VerifyPassword(TestPassword, null));
        }

        [TestMethod]
        public void VerifyPassword_WithEmptyPasswordHash_ThrowsGuardException()
        {
            PasswordManager sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.VerifyPassword(TestPassword, string.Empty));
        }

        [TestMethod]
        public void VerifyPassword_WithValidPasswordAndHash_ReturnsTrue()
        {
            PasswordManager sut = CreateSut();

            bool result = sut.VerifyPassword(TestPassword, TestPasswordHash);

            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void VerifyPassword_WithValidPasswordAndWrongHash_ReturnsFalse()
        {
            PasswordManager sut = CreateSut();

            bool result = sut.VerifyPassword(TestPassword, "1000:e+p5CWM4YMlmAPI=:e+p5CWM4YMlmAPI=");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePassword_PasswordTooShort()
        {
            string testPassword = "5horT4@";
            Assert.IsFalse(PasswordManager.ValidatePassword(testPassword));
        }

        [TestMethod]
        public void ValidatePassword_Password_NoUpperCase()
        {
            string testPassword = "asodifwei3%43jfiwefwef";
            Assert.IsFalse(PasswordManager.ValidatePassword(testPassword));
        }

        [TestMethod]
        public void ValidatePassword_Password_NoLowerCase()
        {
            string testPassword = "HWEINHWENFENF2345@#2FWEFWE#";
            Assert.IsFalse(PasswordManager.ValidatePassword(testPassword));
        }

        [TestMethod]
        public void ValidatePassword_Password_NoNumbers()
        {
            string testPassword = "auBREAntsdgWERgWERGwe@#fdr$";
            Assert.IsFalse(PasswordManager.ValidatePassword(testPassword));
        }

        [TestMethod]
        public void ValidatePassword_Password_AcceptsSpecialCharacters()
        {
            string testPassword = @"owW!@#$%^&*()_+[]\;',./?><:{}|/*-""+`~jdd3 28";
            Assert.IsTrue(PasswordManager.ValidatePassword(testPassword));
        }

        [TestMethod]
        public void ValidatePassword_Password_AcceptsSpaces()
        {
            string testPassword = " eww38sFR35  32#2fwe  ";
            Assert.IsTrue(PasswordManager.ValidatePassword(testPassword));            
        }

        [TestMethod]
        public void ValidatePassword_Password_AcceptsNoSpecialCharcters()
        {
            string testPassword = "PassW0rd1234";
            Assert.IsTrue(PasswordManager.ValidatePassword(testPassword));
        }

        [TestMethod]
        public void ValidatePassword_Password_AcceptsValidPassword()
        {
            string testPassword = "p@SSw0rd2183!%";
            Assert.IsTrue(PasswordManager.ValidatePassword(testPassword));
        }

        private PasswordManager CreateSut()
        {
            return CreateSut(_mockSettings);    
        }

        private PasswordManager CreateSut(ISecuritySettings settings)
        {
            return new PasswordManager(settings);
        }
    }
}