using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Identifix.Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            var claims = (User as ClaimsPrincipal).Claims;
            
            return View(claims);
        }

        [Authorize]
         public ActionResult ChangePassword()
        {
            // TODO: Needs code review. We are passing the claimed UID for now. We will figure out a way to identify the user later.
            var claims = (User as ClaimsPrincipal).Claims;
            var uid = claims.First(x => x.Type == "sub");

             string changePwdURL = string.Format("{0}/User/Default.html#/update?uid={1}&redirectUrl={2}",
                 Startup.InMemoryConfiguration.IdentityServerURL.ToLower().Replace("/identity", ""), uid.Value, Request.Url.AbsoluteUri);

            return Redirect(changePwdURL);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}