using System.Web;
using System.Web.Mvc;

namespace Identifix.IdentityServer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            Guard.IsNotNull(filters, "filters");
            filters.Add(new HandleErrorAttribute());
        }
    }
}
