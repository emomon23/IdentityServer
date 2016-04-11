using Identifix.IdentityServer.Controllers;
using Identifix.IdentityServer.Infrastructure;

namespace Identifix.IdentityServer.Tests.Controllers
{
    public class MvcControllerBaseTestController : MvcControllerBase
    {
        public MvcControllerBaseTestController(IApplicationContext context) : base(context)
        {
        }
    }
}