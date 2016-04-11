using System.Web.Mvc;
using Identifix.IdentityServer.Infrastructure;
using Identifix.IdentityServer.Models;

namespace Identifix.IdentityServer.Controllers
{
    public class HomeController : MvcControllerBase
    {
        private IUserService userService = null;
        public HomeController(IApplicationContext context, IUserService userService) : base(context)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string token, string signin)
        {
            return Redirect($"../User/default.html#/resetPassword?token={ token }&signin={ signin }&expired={ !userService.VerifyPasswordResetToken(token) }");
        }
    }
}