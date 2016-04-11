using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Identifix.IdentityServer.Models.Data;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Identifix.IdentityServer.Infrastructure;

namespace Identifix.IdentityServer.Models
{
    public class UserService : IdentityServer3.Core.Services.IUserService, IUserService
    {
        /// <summary>
        ///  Creates an instance of the User Service
        /// </summary>
        /// <param name="repository"></param>
        public UserService(IUserRepository repository, IPasswordManager passwordManager, IEmailManager emailManager)
        {
            Guard.IsNotNull(repository, "repository");
            Guard.IsNotNull(passwordManager, "passwordManager");
            Repository = repository;
            PasswordManager = passwordManager;
            EmailManager = emailManager;
        }

        /// <summary>
        /// A reference to the injected UserRepository
        /// </summary>
        protected IUserRepository Repository { get; private set; }

        /// <summary>
        /// A reference to the injected PasswordManager
        /// </summary>
        protected IPasswordManager PasswordManager { get; private set; }

        protected IEmailManager EmailManager { get; private set; }

        /// <summary>
        /// Retrieves a list of all countries
        /// </summary>
        /// <returns></returns>
        public IList<Country> GetCountries()
        {
            return Repository.RetrieveCountries();
        }

        /// <summary>
        /// Retrieves a list of all states in given country
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public IList<State> GetStates(string countryCode)
        {
            Guard.IsNotNullOrEmpty(countryCode, "countryCode");
            return Repository.RetrieveStates(countryCode);
        }

        public IList<State> GetStates()
        {
            return Repository.RetrieveStates();
        }

        /// <summary>
        /// Retrieves a list of all states in given country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public IList<State> GetStates(int countryId)
        {
            return Repository.RetrieveStates(countryId);
        }

        public User RegisterNewUser(User user, string password)
        {

            Guard.IsNotNull(user, "user");
            Guard.IsNotNullOrEmpty(password, "password");
            Shop existingShop = Repository.RetrieveShop(user.Shop.Address);
            if (existingShop != null)
            {
                user.Shop = existingShop;
            }
            UserAccount account = Mapper.Map<UserAccount>(user);

            account.Password = PasswordManager.HashPassword(password);
            return Repository.SaveUser(account);
        }

        public User RetrieveUserProfile(int id)
        {
            return Repository.RetrieveUser(id);
        }

        public void UpdateUserProfile(User userProfile)
        {
            UserAccount userAcct = Mapper.Map<UserAccount>(userProfile);

            //We don't want the password to get overridden, so set it to whatever is currently in the db
            var hashedPwd = Repository.RetrieveUser(userProfile.Id).Password;

            userAcct.Password = hashedPwd;
            Repository.SaveUser(userAcct);
        }

        public bool VerifyEmailAddressIsAvaliable(string emailAddress)
        {
            Guard.IsNotNullOrEmpty(emailAddress, "emailAddress");
            return !Repository.CheckIfEmailExists(emailAddress);
        }

        public bool UpdatePasswordForUser(int userId, string currentPassword, string newPassword, string newPasswordConfirm)
        {
            Guard.IsNotNullOrWhiteSpace(currentPassword, "currentPassword");
            Guard.IsNotNullOrWhiteSpace(newPassword, "newPassword");
            Guard.IsNotNullOrWhiteSpace(newPasswordConfirm, "newPasswordConfirm");
            Guard.StringsMatch(newPassword, newPasswordConfirm);

            var user = Repository.RetrieveUser(userId);

            if (user == null)
                throw new ArgumentException($"UserId does not exist: {userId}");

            if (PasswordManager.VerifyPassword(currentPassword, user.Password))
            {
                user.Password = PasswordManager.HashPassword(newPassword);
                Repository.SaveUser(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            return Task.FromResult(0);
        }

        public Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            try
            {
                var user = Repository.RetrieveUser(context.UserName);

                if (user != null && PasswordManager.VerifyPassword(context.Password.Trim(), user.Password))
                {
                    context.AuthenticateResult = new AuthenticateResult(user.Id.ToString(), user.Email);
                }
                else
                {
                    context.AuthenticateResult = new AuthenticateResult("Invalid Username or Password");
                }

            }
            catch (System.Exception exp)
            {
                throw exp;
            }
            return Task.FromResult(0);
        }

        public Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            return Task.FromResult(0);
        }

        public Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            return Task.FromResult(0);
        }

        public Task SignOutAsync(SignOutContext context)
        {
            return Task.FromResult(0);
        }

        //public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            {
                var identity = new ClaimsIdentity();

                User user = null;
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                    user = Repository.RetrieveUser(context.Subject.Identity.Name);
                else
                {
                    // get the sub claim
                    var claim = context.Subject.FindFirst(item => item.Type == "sub");
                    if (claim != null)
                    {
                        int userId = int.Parse(claim.Value);
                        user = Repository.RetrieveUser(userId);
                    }
                }

                if (user != null)
                {
                    string backOfficeShopId = user.Shop == null
                        ? "NONE"
                        : string.IsNullOrEmpty(user.Shop.ShopId) ? "NONE" : user.Shop.ShopId;

                    identity.AddClaims(new[]
                    {
                    new Claim(Constants.ClaimTypes.FamilyName, user.LastName),
                    new Claim(Constants.ClaimTypes.GivenName, user.FirstName),
                    new Claim(Constants.ClaimTypes.Email, user.Email),
                    new Claim(Constants.ClaimTypes.Role, "Geek"),
                    new Claim("back_office_shop_id", backOfficeShopId, "profile"), 
                    // .. other claims.
                });
                }

                context.IssuedClaims = identity.Claims; //<- MAKE SURE you add the claims here
                return Task.FromResult(identity.Claims);
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }

        public string GeneratePasswordResetLink(string emailAddress, string redirectSignInToken)
        {
            Guard.IsNotNullOrWhiteSpace(emailAddress, "emailAddress");
            Guard.IsNotNullOrWhiteSpace(redirectSignInToken, "redirectSignInToken");
            string emailFileName = "";
            var user = Repository.RetrieveUser(emailAddress.Trim());
            if (user != null)
            {
                string token = Repository.SaveResetPasswordData(user.Id);

                string resetPasswordUrl = $"{ AppSettings.IdentifixSSOUrl }Home/ResetPassword?token={ token  }&signin={ redirectSignInToken }";

                emailFileName = EmailManager.SendEmail($"{ user.Email }", AppSettings.IdentifixSystemEmailAddress, $"Password Reset Link. EXPIRES IN { AppSettings.PasswordResetExpirationAmount } MINUTES", $"Reset Link: { resetPasswordUrl }");                
            }

            return emailFileName;
        }

        public bool SavePasswordFromReset(string token, string newPassword, string newPasswordConfirm)
        {
            Guard.IsNotNullOrWhiteSpace(token, "Reset Password Token");
            Guard.IsNotNullOrWhiteSpace(newPassword, "Password Reset - NewPassword");
            Guard.IsNotNullOrWhiteSpace(newPasswordConfirm, "Password Reset - NewPasswordConfirm");
            Guard.StringsMatch(newPassword, newPasswordConfirm);

            int? userId = Repository.IsValidToken(token);
            if (userId != null)
            {
                var user = Repository.RetrieveUser(userId.Value);
                user.Password = PasswordManager.HashPassword(newPassword);
                Repository.SaveUser(user); 
                Repository.ExpireResetPasswordTokenForUser(user.Id);
                return true;
            }

            return false;
        }

        public bool VerifyPasswordResetToken(string token)
        {
            Guard.IsNotNullOrWhiteSpace(token, "Password Reset Token");

            return Repository.IsValidToken(token).HasValue;
        }
    }
}