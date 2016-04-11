using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Identifix.IdentityServer.Infrastructure;
using Identifix.IdentityServer.Models;
using System.Security.Claims;
using System.Web.Script.Serialization;

namespace Identifix.IdentityServer.API
{
    public class SecurityController : BaseApiController
    {
        private IUserService userService = null;

        public SecurityController(IApplicationContext context, IUserService userService) : base(context)
        {
            this.userService = userService;
        }

        [HttpPost]
        public APIResult RegisterNewUser(NewUserRegistration newUser)
        {
            APIResult result = new APIResult() { IsSuccessful = true };

            try
            {
                User user = Mapper.Map<User>(newUser);
                user.Shop = Mapper.Map<Shop>(newUser);
                user.Shop.Address = Mapper.Map<Address>(newUser);

                userService.RegisterNewUser(user, newUser.Password);
            }
            catch (Exception exp)
            {
                result = APIResultFactory.CreateAPIResult(exp);
            }

            return result;
        }

        [HttpPost]
        public APIResult UpdateUserProfile(UserProfile updatedProfile)
        {
            try
            {
                User user = Mapper.Map<User>(updatedProfile);
                user.Shop = Mapper.Map<Shop>(updatedProfile);
                user.Shop.Address = Mapper.Map<Address>(updatedProfile);

                userService.UpdateUserProfile(user);
                return APIResultFactory.CreateAPIResult(true);
            }
            catch (Exception exp)
            {
                return APIResultFactory.CreateAPIResult(exp);
            }
        }

        [HttpGet]
        public APIResult RetrieveUserProfile([FromUri]SubmittedData request)
        {
            try
            {
                int userId = 0;
                if (int.TryParse(request.Payload.ToString(), out userId))
                {
                    User user = userService.RetrieveUserProfile(userId);
                    UserProfile result = Mapper.Map<UserProfile>(user);

                    return APIResultFactory.CreateAPIResult(true, "", result);
                }

                return APIResultFactory.CreateAPIResult(false, "request.payload is not a valid userId");
            }
            catch (Exception exp)
            {
                return APIResultFactory.CreateAPIResult(exp);
            }
        }

        [HttpPost]
        public APIResult EmailValidation(SubmittedData data)
        {
            try
            {
                if (data.Payload == null)
                {
                    return new APIResult() { IsSuccessful = true };
                }

                string[] splitPayload = ((string)data.Payload).Split(',');
                string email = splitPayload[0];
                string originalEmail = splitPayload.Length > 1 ? splitPayload[1] : "";

                bool userNameIsUnique = email == originalEmail ? true : userService.VerifyEmailAddressIsAvaliable(email);

                var result = new APIResult()
                {
                    IsSuccessful = true,
                    Payload = userNameIsUnique,
                    ErrorMessage =
                        userNameIsUnique ? string.Empty : string.Format("Email Address '{0}' is not unique", email)
                };

                return result;
            }
            catch (Exception exp)
            {
                return APIResultFactory.CreateAPIResult(exp);
            }
        }

        [HttpPost]
        public APIResult ChangePassword(ChangePasswordModel model)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                result = userService.UpdatePasswordForUser(model.UserId, model.CurrentPassword, model.NewPassword, model.NewPasswordConfirm);
            }

            return APIResultFactory.CreateAPIResult(result, result ? "" : "Your password could not be updated. Please re-fill out the form and re-submit.");
        }

        [HttpPost]
        public APIResult ValidatePassword(SubmittedData data)
        {
            return new APIResult()
            {
                IsSuccessful = true,
                Payload = PasswordManager.ValidatePassword(data.Payload.ToString()),
                ErrorMessage = "The password must be at least 8 characters long with one lower case letter, one uppercase letter and one number."
            };
        }

        [HttpPost]
        public APIResult GeneratePasswordResetLink(GeneratePasswordResetLinkData data)
        {            
            if (!string.IsNullOrWhiteSpace(data?.ClientIdRedirect) && !string.IsNullOrWhiteSpace(data?.UsersEmailAddress))
            {
                var emailFileName = userService.GeneratePasswordResetLink(data.UsersEmailAddress.Trim(), data.ClientIdRedirect.Trim());
                return APIResultFactory.CreateAPIResult(true, payload: emailFileName);
            }
            else
            {
                return APIResultFactory.CreateAPIResult(false, "We are unable to produce a password reset link with the information provided. Please try again or contact support for help.");
            }
        }

        [HttpPost]
        public APIResult ResetPassword(ResetPasswordModel model)
        {
            if (!string.IsNullOrWhiteSpace(model?.Token) && !string.IsNullOrWhiteSpace(model?.SignInToken) && !string.IsNullOrWhiteSpace(model?.NewPassword) && !string.IsNullOrWhiteSpace(model?.NewPasswordConfirm))
            {
                if(userService.SavePasswordFromReset(model.Token.Trim(), model.NewPassword, model.NewPasswordConfirm))
                {
                    string url = $"{ Request.RequestUri.GetLeftPart(UriPartial.Authority) }/identity/login?signin={model.SignInToken.Trim()}";
                    return APIResultFactory.CreateAPIResult(true, payload: url);
                }
                else
                {
                    return APIResultFactory.CreateAPIResult(false, "There was an error updating your password. Please try again or contact support for help.");
                }
            }
            else
            {
                return APIResultFactory.CreateAPIResult(false, "We are unable to reset your password with the information provided. Please try again or contact support for help.");
            }
        }
    }
}
