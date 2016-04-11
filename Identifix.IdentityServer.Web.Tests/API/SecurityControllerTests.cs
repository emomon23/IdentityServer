using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Identifix.IdentityServer.Controllers;
using Identifix.IdentityServer.API;
using Identifix.IdentityServer.Tests.Controllers;
using Identifix.IdentityServer.Models;
using Rhino.Mocks;
using System.Net.Http;

namespace Identifix.IdentityServer.Tests.API
{
    [TestClass]
    public class SecurityControllerTests
    {
        private IUserService _userServiceMock { get; set; }
        private ControllerMocks _controllerMocks { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _userServiceMock = MockRepository.GenerateMock<IUserService>();
            _controllerMocks = new ControllerMocks();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _userServiceMock = null;
            _controllerMocks = null;
        }

        [TestMethod]
        public void ChangePassword_Valid_Arguements()
        {
            _userServiceMock.Stub(m => m.UpdatePasswordForUser(Arg<int>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            var securityController = new SecurityController(_controllerMocks.Context, _userServiceMock);

            var model = new ChangePasswordModel() { UserId = 94, CurrentPassword = "Current_P@ssw0rd", NewPassword = "N3wP@ssword", NewPasswordConfirm = "N3wP@ssword" };

            bool result = securityController.ChangePassword(model).IsSuccessful;
            Assert.IsTrue(result);
        }

        /// <summary>
        /// This is the scenario where valid arguements are passed to the user service ChangePassword routine. The user service ChangePassword routine then finds a issue
        /// such as user id doesnt exist in the DB and does not update the password and returns false. We need to assert that the controller reports back to the caller
        /// that the password was never updated.
        /// </summary>
        [TestMethod]        
        public void ChangePassword_Valid_Arguements_UserServiceDoesntUpdatePassword()
        {
            _userServiceMock.Stub(m => m.UpdatePasswordForUser(Arg<int>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(false);
            var securityController = new SecurityController(_controllerMocks.Context, _userServiceMock);

            var model = new ChangePasswordModel() { UserId = -65, CurrentPassword = "Current_P@ssw0rd", NewPassword = "N3wP@ssword", NewPasswordConfirm = "N3wP@ssword" };

            var result = securityController.ChangePassword(model);
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ErrorMessage.Trim().Length > 0);
        }

        [TestMethod]
        public void GeneratePasswordResetLink_Null_Arguements()
        {
            var securityController = new SecurityController(_controllerMocks.Context, _userServiceMock);

            var model = new GeneratePasswordResetLinkData() { ClientIdRedirect = null, UsersEmailAddress = null };

            var result = securityController.GeneratePasswordResetLink(model);
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ErrorMessage.Trim().Length > 0);
        }

        [TestMethod]
        public void GeneratePasswordResetLink_Whitespace_Arguements()
        {
            var securityController = new SecurityController(_controllerMocks.Context, _userServiceMock);

            var model = new GeneratePasswordResetLinkData() { ClientIdRedirect = "    ", UsersEmailAddress = "       " };

            var result = securityController.GeneratePasswordResetLink(model);
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ErrorMessage.Trim().Length > 0);
        }

        [TestMethod]
        public void GeneratePasswordResetLink_Valid_Arguements()
        {
            var securityController = new SecurityController(_controllerMocks.Context, _userServiceMock);

            var model = new GeneratePasswordResetLinkData() { ClientIdRedirect = Guid.NewGuid().ToString(), UsersEmailAddress = "Testing@Identifix.com" };

            var result = securityController.GeneratePasswordResetLink(model);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);            
        }

        [TestMethod]
        public void ResetPassword_Invalid_Arguements()
        {
            var securityController = new SecurityController(_controllerMocks.Context, _userServiceMock);

            var model = new ResetPasswordModel() { Token = null, NewPassword = null, NewPasswordConfirm = null, SignInToken = null  };

            var result = securityController.ResetPassword(model);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ErrorMessage.Trim().Length > 0);
        }

        [TestMethod]
        public void ResetPassword_InvalidWhiteSpace_Arguements()
        {
            var securityController = new SecurityController(_controllerMocks.Context, _userServiceMock);

            var model = new ResetPasswordModel() { Token = " ", NewPassword = "    ", NewPasswordConfirm = "    ", SignInToken = "   " };

            var result = securityController.ResetPassword(model);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ErrorMessage.Trim().Length > 0);
        }

        [TestMethod]
        public void ResetPassword_Valid_Arguements()
        {
            string fakeHost = "http://localhost:7382";
            string signInToken = Guid.NewGuid().ToString();
            _userServiceMock.Stub(x => x.SavePasswordFromReset(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            
            var securityController = new SecurityController(_controllerMocks.Context, _userServiceMock);
            var mockedRequest = MockRepository.GenerateMock<HttpRequestMessage>();
            mockedRequest.RequestUri = new Uri($"{ fakeHost }/test/cool?myparam=2382");
            securityController.Request = mockedRequest;

            var model = new ResetPasswordModel() { Token = Guid.NewGuid().ToString(), NewPassword = "Password1", NewPasswordConfirm = "Password1", SignInToken = signInToken };

            var result = securityController.ResetPassword(model);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual($"{fakeHost}/identity/login?signin={signInToken}", result.Payload.ToString());
        }

        [TestMethod]
        public void ResetPassword_Repository_Fails()
        {
            _userServiceMock.Stub(x => x.SavePasswordFromReset(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(false);

            var securityController = new SecurityController(_controllerMocks.Context, _userServiceMock);

            var model = new ResetPasswordModel() { Token = Guid.NewGuid().ToString(), NewPassword = "Password1", NewPasswordConfirm = "Password1", SignInToken = Guid.NewGuid().ToString() };

            var result = securityController.ResetPassword(model);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ErrorMessage.Trim().Length > 0);
        }
    }
}
