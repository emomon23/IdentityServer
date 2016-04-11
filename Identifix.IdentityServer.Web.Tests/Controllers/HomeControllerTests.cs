using System.Web.Mvc;
using Identifix.IdentityServer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Identifix.IdentityServer.Models;
using Rhino.Mocks;

namespace Identifix.IdentityServer.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private IUserService _userServiceMock { get; set; }

        private ControllerMocks _mocks;
        [TestInitialize]
        public void Initialize()
        {
            _userServiceMock = MockRepository.GenerateMock<IUserService>();
            _mocks = new ControllerMocks();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mocks = null;
        }

        [TestMethod]
        public void Index_ReturnsDefaultView()
        {
            HomeController sut = CreateSut();

            ViewResult result = sut.Index();

            Assert.AreEqual(result.ViewName, string.Empty);
        }

        [TestMethod]
        public void ResetPassword_ReturnsExpiredRedirect()
        {
            HomeController sut = CreateSut();
            string token = "6e37d2e2-6e89-4ad4-88bc-4e65dc5bd752";
            string signIn = "b62b15f2-c32d-4965-bbf0-9a89f48b7bad";
            bool IsValidToken = true;

            _userServiceMock.Stub(x => x.VerifyPasswordResetToken(Arg<string>.Is.Anything)).Return(IsValidToken);
            ActionResult result = sut.ResetPassword(token,signIn);

            Assert.IsNotNull(result);
            Assert.AreEqual(((RedirectResult)result).Url, $"../User/default.html#/resetPassword?token={ token }&signin={ signIn }&expired=False");
        }

        [TestMethod]
        public void ResetPassword_ReturnsNonExpiredRedirect()
        {
            HomeController sut = CreateSut();
            string token = "6e37d2e2-6e89-4ad4-88bc-4e65dc5bd752";
            string signIn = "b62b15f2-c32d-4965-bbf0-9a89f48b7bad";
            bool IsValidToken = false;

            _userServiceMock.Stub(x => x.VerifyPasswordResetToken(Arg<string>.Is.Anything)).Return(IsValidToken);
            ActionResult result = sut.ResetPassword(token, signIn);

            Assert.IsNotNull(result);
            Assert.AreEqual(((RedirectResult)result).Url, $"../User/default.html#/resetPassword?token={ token }&signin={ signIn }&expired=True");
        }


        private HomeController CreateSut()
        {
            return new HomeController(_mocks.Context, _userServiceMock);
        }

    }
}