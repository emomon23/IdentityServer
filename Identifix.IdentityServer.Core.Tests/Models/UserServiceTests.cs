using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Identifix.IdentityServer.Models;
using Identifix.IdentityServer.Models.Data;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Identifix.IdentityServer.Infrastructure;

namespace Identifix.IdentityServer.Tests.Models
{
    [TestClass]
    public class UserServiceTests
    {
        const string SampleHash = "1000:vzR0cmZHB4c9wDZDrvSc780Fr42FoCYz:0+BEpeItf9UoAq29Nva3G72FCG1KbA5ejV8kkdDkkSekj4FkmKjbW3SeRt41uSrentjQjgTdzzKRGaZZ";

        private IUserRepository _mockRepository;
        private IPasswordManager _mockPasswordManager;
        private IEmailManager _mockEmailManager;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = MockRepository.GenerateMock<IUserRepository>();
            _mockPasswordManager = MockRepository.GenerateMock<IPasswordManager>();
            _mockEmailManager = MockRepository.GenerateMock<IEmailManager>();
            MappingConfig.Configure();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mockRepository = null;
            _mockPasswordManager = null;
            _mockEmailManager = null;
        }

        [TestMethod]
        public void Instantiation_WithNullRepository_ThrowsGuardException()
        {
            CustomAssert.Throws<GuardException>(() => CreateSut(null));
        }

        [TestMethod]
        public void Instantiation_WithNullPasswordManager_ThrowsGuardException()
        {
            CustomAssert.Throws<GuardException>(() => CreateSut(_mockRepository, null));
        }

        [TestMethod]
        public void GetCountries_RetrievesCountriesFromRepository()
        {
            _mockRepository.Expect(mock => mock.RetrieveCountries()).Repeat.Once().Return(new List<Country>());
            UserService sut = CreateSut();

            sut.GetCountries();

            _mockRepository.VerifyAllExpectations();
        }

        [TestMethod]
        public void GetStates_WithValidCountryCode_RetrievesStatesFromRepository()
        {
            const string countryCode = "US";
            _mockRepository.Expect(mock => mock.RetrieveStates(countryCode)).Repeat.Once().Return(new List<State>());
            UserService sut = CreateSut();

            sut.GetStates(countryCode);

            _mockRepository.VerifyAllExpectations();
        }

        [TestMethod]
        public void GetStates_WithNullCountryCode_ThrowsGuardException()
        {
            UserService sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.GetStates(null));
        }
        
        [TestMethod]
        public void GetStates_WithEmptyCountryCode_ThrowsGuardException()
        {
            UserService sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.GetStates(string.Empty));
        }

        [TestMethod]
        public void GetStates_WithValidCountryId_RetrievesStatesFromRepository()
        {
            const int countryId = 1;
            _mockRepository.Expect(mock => mock.RetrieveStates(countryId)).Repeat.Once().Return(new List<State>());
            UserService sut = CreateSut();

            sut.GetStates(countryId);

            _mockRepository.VerifyAllExpectations();
        }
        
        [TestMethod]
        public void GetStates_WithNoArguments_RetrievesAllStatesFromRepository()
        {
            _mockRepository.Expect(mock => mock.RetrieveStates()).Repeat.Once().Return(new List<State>());
            UserService sut = CreateSut();

            sut.GetStates();

            _mockRepository.VerifyAllExpectations();
        }


        [TestMethod]
        public void VerifyEmailAddressIsAvailable_WithNullEmailAddress_ThrowsGuardException()
        {
            UserService sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.VerifyEmailAddressIsAvaliable(null));
        }

        [TestMethod]
        public void VerifyEmailAddressIsAvailable_WithEmptyEmailAddress_ThrowsGuardException()
        {
            UserService sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.VerifyEmailAddressIsAvaliable(String.Empty));
        }

        [TestMethod]
        public void VerifyEmailAddressIsAvailable_WithValidEmailAddress_ChecksRepositoryForExistence()
        {
            const string email = "foo@mail.com";
            _mockRepository.Expect(mock => mock.CheckIfEmailExists(Arg<string>.Is.Anything)).Return(true);
            UserService sut = CreateSut();

            sut.VerifyEmailAddressIsAvaliable(email);

            _mockRepository.VerifyAllExpectations();
        }

        [TestMethod]
        public void VerifyEmailAddressIsAvailable_WithExistingEmailAddress_ReturnsFalse()
        {
            const string email = "foo@mail.com";
            _mockRepository.Expect(mock => mock.CheckIfEmailExists(Arg<string>.Is.Anything)).Return(true);
            UserService sut = CreateSut();

            bool result = sut.VerifyEmailAddressIsAvaliable(email);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VerifyEmailAddressIsAvailable_WithNonExistingEmailAddress_ReturnsTrue()
        {
            const string email = "foo@mail.com";
            _mockRepository.Expect(mock => mock.CheckIfEmailExists(Arg<string>.Is.Anything)).Return(false);
            UserService sut = CreateSut();

            bool result = sut.VerifyEmailAddressIsAvaliable(email);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RegisterNewUser_WithNullUser_ThrowsGuardException()
        {
            IUserService sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.RegisterNewUser(null, "password"));
        }

        [TestMethod]
        public void RegisterNewUser_WithNullPassword_ThrowsGuardException()
        {
            User user = (new UserBuilder()).AsDefault();
            IUserService sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.RegisterNewUser(user, null));
        }

        [TestMethod]
        public void RegisterNewUser_WithEmptyPassword_ThrowsGuardException()
        {
            User user = (new UserBuilder()).AsDefault();
            IUserService sut = CreateSut();

            CustomAssert.Throws<GuardException>(() => sut.RegisterNewUser(user, string.Empty));
        }
        
        [TestMethod]
        public void RegisterNewUser_ValidUserAndPassword_ChecksForExistingShop()
        {
            User user = (new UserBuilder()).AsDefault();
            user.Shop = new Shop
            {
                Address = new Address()
            };
            const string password = "password";
            IUserService sut = CreateSut();
            _mockRepository.Expect(mock => mock.RetrieveShop(Arg<Address>.Is.Anything)).Return((Shop) null);

            sut.RegisterNewUser(user, password);

            _mockRepository.VerifyAllExpectations();
        }

        [TestMethod]
        public void RegisterNewUser_ValidUserAndPasswordAndShopAlreadyExists_UserShopIsUpdatedToExistingShop()
        {
            Shop postedShop = new Shop
            {
                Address = new Address()
            };
            Shop existingShop = new Shop
            {
                Id = 123,
                Address = new Address()
            };
            User user = (new UserBuilder()).AsDefault();
            user.Shop = postedShop;
            const string password = "password";
            IUserService sut = CreateSut();
            _mockRepository.Expect(mock => mock.RetrieveShop(Arg<Address>.Is.Anything)).Return(existingShop);

            sut.RegisterNewUser(user, password);

            Assert.AreSame(existingShop, user.Shop);
        }


        [TestMethod]
        public void RegisterNewUser_ValidUserAndPassword_HashesPassword()
        {
            User user = (new UserBuilder()).AsDefault();
            user.Shop = new Shop
            {
                Address = new Address()
            };
            const string password = "password";
            IUserService sut = CreateSut();
            _mockRepository.Expect(mock => mock.RetrieveShop(Arg<Address>.Is.Anything)).Return((Shop)null);
            _mockPasswordManager.Expect(mock => mock.HashPassword(Arg<string>.Is.Anything)).Return(SampleHash); 

            sut.RegisterNewUser(user, password);

            _mockPasswordManager.VerifyAllExpectations();
        }
        
        [ExpectedException(typeof(GuardException))]
        public void UpdatePasswordForUser_NullCurrentPassword()
        {
            UserService sut = CreateSut();
            sut.UpdatePasswordForUser(554, null, "N3wP@ssword", "N3wP@ssword");
        }

        [TestMethod]
        [ExpectedException(typeof(GuardException))]
        public void UpdatePasswordForUser_NullNewPassword()
        {
            UserService sut = CreateSut();
            sut.UpdatePasswordForUser(78468, "Curr3ntP@ssw0rd", null, "N3wP@ssword");
        }

        [TestMethod]
        [ExpectedException(typeof(GuardException))]
        public void UpdatePasswordForUser_NullNewPasswordConfirm()
        {
            UserService sut = CreateSut();
            sut.UpdatePasswordForUser(5623, "Curr3ntP@ssw0rd", "N3wP@ssword", null);
        }

        [TestMethod]
        [ExpectedException(typeof(GuardException))]
        public void UpdatePasswordForUser_WhitespaceCurrentPassword()
        {
            UserService sut = CreateSut();
            sut.UpdatePasswordForUser(554, "        ", "N3wP@ssword", "N3wP@ssword");
        }

        [TestMethod]
        [ExpectedException(typeof(GuardException))]
        public void UpdatePasswordForUser_WhitespaceNewPassword()
        {
            UserService sut = CreateSut();
            sut.UpdatePasswordForUser(78468, "Curr3ntP@ssw0rd", "             ", "N3wP@ssword");
        }

        [TestMethod]
        [ExpectedException(typeof(GuardException))]
        public void UpdatePasswordForUser_WhitespaceNewPasswordConfirm()
        {
            UserService sut = CreateSut();
            sut.UpdatePasswordForUser(5623, "Curr3ntP@ssw0rd", "N3wP@ssword", "            ");
        }

        [TestMethod]
        public void UpdatePasswordForUser_ValidArguements_UpdatesPassword()
        {
            int userId = 7183;
            string newPassword = "N3wP@ssword";
            string newPasswordConfirmation = "N3wP@ssword";
            string currentPassword = "Curr3ntP@ssw0rd";
            var fakeUser = new UserAccount() { Id = userId };
            string fakeHashedPassword = Guid.NewGuid().ToString();
            

            _mockRepository.Stub(m => m.RetrieveUser(Arg<int>.Is.Anything)).Return(fakeUser);            
            _mockPasswordManager.Stub(m => m.VerifyPassword(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(true);            
            _mockPasswordManager.Stub(m => m.HashPassword(Arg<string>.Is.Anything)).Return(fakeHashedPassword);

            UserService sut = CreateSut(_mockRepository);
            bool result = sut.UpdatePasswordForUser(userId, currentPassword, newPassword, newPasswordConfirmation);

            Assert.AreEqual(fakeUser.Password, fakeHashedPassword);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdatePasswordForUser_UserIdDoesntExist_ThrowsExceptions()
        {
            int userId = -5;

            _mockRepository.Stub(m => m.RetrieveUser(Arg<int>.Is.Anything)).Return(null);            

            UserService sut = CreateSut(_mockRepository);
            bool result = sut.UpdatePasswordForUser(userId, "foo", "foo1", "foo1");         
        }

        [TestMethod]
        public void UpdatePasswordForUser_WhenCurrentPasswordIsIncorrect_ReturnsFalse()
        {
            int userId = 7183;
            string newPassword = "N3wP@ssword";
            string newPasswordConfirmation = "N3wP@ssword";
            string currentPassword = "Curr3ntP@ssw0rd";
            var fakeUser = new UserAccount() { Id = userId };
            string fakeHashedPassword = Guid.NewGuid().ToString();


            _mockRepository.Stub(m => m.RetrieveUser(Arg<int>.Is.Anything)).Return(fakeUser);
            _mockPasswordManager.Stub(m => m.VerifyPassword(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(false);
            _mockPasswordManager.Stub(m => m.HashPassword(Arg<string>.Is.Anything)).Return(fakeHashedPassword);

            UserService sut = CreateSut(_mockRepository);
            bool result = sut.UpdatePasswordForUser(userId, currentPassword, newPassword, newPasswordConfirmation);
            
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetProfileDataAsync_WhenIdentityNameIsSet_RetrievesUserByUserName()
        {
            UserAccount testAccount = CreateTestUserAccount();
            List<Claim> claims  = new List<Claim> {new Claim(ClaimTypes.Name, "TestName")};
            ClaimsIdentity identity = new ClaimsIdentity(claims);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            ProfileDataRequestContext context = new ProfileDataRequestContext
            {
                Subject = principal
            };
            _mockRepository.Expect(mock => mock.RetrieveUser(Arg<string>.Is.Anything)).Return(testAccount);

            UserService sut = CreateSut();

            sut.GetProfileDataAsync(context);

            _mockRepository.VerifyAllExpectations();
        }

        [TestMethod]
        public void GetProfileDataAsync_WhenIdentityNameIsSet_ReturnsExpectedClaims()
        {
            UserAccount testAccount = CreateTestUserAccount();
            List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, "TestName") };
            ClaimsIdentity identity = new ClaimsIdentity(claims);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            ProfileDataRequestContext context = new ProfileDataRequestContext
            {
                Subject = principal
            };
            _mockRepository.Stub(mock => mock.RetrieveUser(Arg<string>.Is.Anything)).Return(testAccount);

            UserService sut = CreateSut();

            sut.GetProfileDataAsync(context);

            Assert.IsNotNull(
                context.IssuedClaims.SingleOrDefault(
                    claim => claim.Type == Constants.ClaimTypes.FamilyName && claim.Value == testAccount.LastName));
            Assert.IsNotNull(
                context.IssuedClaims.SingleOrDefault(
                    claim => claim.Type == Constants.ClaimTypes.GivenName && claim.Value == testAccount.FirstName));
            Assert.IsNotNull(
                context.IssuedClaims.SingleOrDefault(
                    claim => claim.Type == Constants.ClaimTypes.Email && claim.Value == testAccount.Email));
        }

        [TestMethod]
        public void GetProfileDataAsync_WhenIdentitySubjectIsSet_RetrievesUserByUserId()
        {
            UserAccount testAccount = CreateTestUserAccount();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, string.Empty),
                new Claim("sub", "123")
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            ProfileDataRequestContext context = new ProfileDataRequestContext
            {
                Subject = principal
            };
            _mockRepository.Expect(mock => mock.RetrieveUser(Arg<int>.Is.Anything)).Return(testAccount);

            UserService sut = CreateSut();

            sut.GetProfileDataAsync(context);

            _mockRepository.VerifyAllExpectations();
        }
        
        [TestMethod]
        public void GetProfileDataAsync_WhenIdentitySubjectIsSet_ReturnsExpectedClaims()
        {
            UserAccount testAccount = CreateTestUserAccount();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, string.Empty),
                new Claim("sub", "123")
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            ProfileDataRequestContext context = new ProfileDataRequestContext
            {
                Subject = principal
            };
            _mockRepository.Stub(mock => mock.RetrieveUser(Arg<int>.Is.Anything)).Return(testAccount);

            UserService sut = CreateSut();

            sut.GetProfileDataAsync(context);

            Assert.IsNotNull(
                context.IssuedClaims.SingleOrDefault(
                    claim => claim.Type == Constants.ClaimTypes.FamilyName && claim.Value == testAccount.LastName));
            Assert.IsNotNull(
                context.IssuedClaims.SingleOrDefault(
                    claim => claim.Type == Constants.ClaimTypes.GivenName && claim.Value == testAccount.FirstName));
            Assert.IsNotNull(
                context.IssuedClaims.SingleOrDefault(
                    claim => claim.Type == Constants.ClaimTypes.Email && claim.Value == testAccount.Email));
        }

        [TestMethod]
        public void GetProfileDataAsync_WhenNeitherNameOrSubjectIsSet_SetsEmptyClaims()
        {
            UserAccount testAccount = CreateTestUserAccount();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, string.Empty)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            ProfileDataRequestContext context = new ProfileDataRequestContext
            {
                Subject = principal
            };

            UserService sut = CreateSut();

            sut.GetProfileDataAsync(context);
            Assert.AreEqual(0, context.IssuedClaims.Count());
        }

        [TestMethod]
        public void GeneratePasswordResetLink_Null_InvalidArguements_ThrowsGuardException()
        {
            UserService sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.GeneratePasswordResetLink(null, null));
        }

        [TestMethod]
        public void GeneratePasswordResetLink_Whitespace_InvalidArguements_ThrowsGuardException()
        {
            UserService sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.GeneratePasswordResetLink("        ", "  "));
        }

        [TestMethod]
        public void GeneratePasswordResetLink_ValidUser()
        {
            var testUser = CreateTestUserAccount();
            UserService sut = CreateSut();
            string token = "23442-saf-343-3g-5355-g3";
            string signIn = "45g-45g-4g4g4g-45g45-ggsd";
            string passwordResetUrl = $"{ AppSettings.IdentifixSSOUrl }Home/ResetPassword?token={ token  }&signin={ signIn }";
            _mockRepository.Stub(x => x.RetrieveUser(Arg<string>.Is.Anything)).Return(CreateTestUserAccount());
            _mockRepository.Stub(x => x.SaveResetPasswordData(Arg<int>.Is.Anything)).Return(token);
            sut.GeneratePasswordResetLink(testUser.Email, signIn);

            _mockEmailManager.AssertWasCalled(x => x.SendEmail(testUser.Email, AppSettings.IdentifixSystemEmailAddress, $"Password Reset Link. EXPIRES IN { AppSettings.PasswordResetExpirationAmount } MINUTES", $"Reset Link: { passwordResetUrl }"));
        }

        [TestMethod]
        public void VerifyPasswordResetToken_ValidToken()
        {            
            _mockRepository.Stub(x => x.IsValidToken(Arg<string>.Is.Anything)).Return(39);

            var sut = CreateSut();
            var IsValidtoken = sut.VerifyPasswordResetToken("234234-2f-3w4-3f-wsf-23-2sdf");

            Assert.IsTrue(IsValidtoken);
        }

        [TestMethod]
        public void VerifyPasswordResetToken_InvalidToken()
        {
            _mockRepository.Stub(x => x.IsValidToken(Arg<string>.Is.Anything)).Return(null);

            var sut = CreateSut();
            var IsValidtoken = sut.VerifyPasswordResetToken("INVALID");

            Assert.IsFalse(IsValidtoken);
        }

        [TestMethod]
        public void VerifyPasswordResetToken_Whitespace_InvalidArguement_ThrowsGuardException()
        {
            var sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.VerifyPasswordResetToken("        "));
        }

        [TestMethod]
        public void VerifyPasswordResetToken_Null_InvalidArguement_ThrowsGuardException()
        {
            var sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.VerifyPasswordResetToken(null));
        }

        [TestMethod]
        public void SavePasswordFromReset_Null_InvalidArguements_ThrowsGuardException()
        {
            var sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.SavePasswordFromReset(null, null, null));
        }

        [TestMethod]
        public void SavePasswordFromReset_WhiteSpace_InvalidArguements_ThrowsGuardException()
        {
            var sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.SavePasswordFromReset("     ", "      ", "     "));
        }

        [TestMethod]
        public void SavePasswordFromReset_NonMatchingPasswords_ThrowsGuardException()
        {
            var sut = CreateSut();
            CustomAssert.Throws<GuardException>(() => sut.SavePasswordFromReset("23424-2-f-4f-3-3v535", "TestingPassword1", "TestingPassword2"));
        }

        [TestMethod]
        public void SavePasswordFromReset_InvalidToken()
        {
            var sut = CreateSut();
            _mockRepository.Stub(x => x.RetrieveUser(Arg<int>.Is.Anything)).Return(null);
            var result = sut.SavePasswordFromReset("32fF4f-34-3434-f4-34f34", "Passw0rd", "Passw0rd");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SavePasswordFromReset_UpdatesPassword()
        {
            var sut = CreateSut();
            var testUser = CreateTestUserAccount();
            var testHashPassword = "34bufn3u4f34f3#$F#344feffer";

            _mockRepository.Stub(x => x.IsValidToken(Arg<string>.Is.Anything)).Return(testUser.Id);
            _mockRepository.Stub(x => x.RetrieveUser(Arg<int>.Is.Anything)).Return(testUser);
            _mockRepository.Stub(x => x.SaveUser(Arg<UserAccount>.Is.Anything)).Return(testUser);

            _mockPasswordManager.Stub(x => x.HashPassword(Arg<string>.Is.Anything)).Return(testHashPassword);
            
            _mockRepository.Expect(x => x.ExpireResetPasswordTokenForUser(testUser.Id));
            
            var result = sut.SavePasswordFromReset("32fF4f-34-3434-f4-34f34", "Passw0rd", "Passw0rd");

            _mockRepository.AssertWasCalled(x => x.SaveUser(testUser));
            Assert.IsTrue(result);
            Assert.AreEqual(testHashPassword, testUser.Password);
            _mockRepository.VerifyAllExpectations();            
        }


        private UserService CreateSut()
        {
            return CreateSut(_mockRepository);
        }

        private UserService CreateSut(IUserRepository repository)
        {
            return CreateSut(repository, _mockPasswordManager);
        }

        private UserService CreateSut(IUserRepository repository, IPasswordManager passwordManager)
        {
            return new UserService(repository, passwordManager, _mockEmailManager);
        }

        private UserAccount CreateTestUserAccount()
        {
            return new UserAccount
            {
                Shop = new Shop
                {
                    Id = 1,
                    Name = "Test Shop",
                    ShopId = "SHOP001"
                },
                ShopId = 1,
                Id = 2,
                Password = "PASSWORD",
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "e@mail.com"
            };
        }
    }
}