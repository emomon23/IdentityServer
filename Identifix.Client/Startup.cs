using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Web.Helpers;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace Identifix.Client
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            //Configuration
            InMemoryConfiguration.IdentityServerURL = ConfigurationManager.AppSettings["IdentityServerURL"];
            InMemoryConfiguration.IdentityServerRedirectUrl = ConfigurationManager.AppSettings["IdentityServerRedirectUrl"];

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = InMemoryConfiguration.IdentityServerURL,
                RedirectUri = InMemoryConfiguration.IdentityServerRedirectUrl,
                Scope = "openid profile roles",
                ClientId = "idc",
                ResponseType = "id_token",
                SignInAsAuthenticationType = "Cookies"
            });

            // Transforms claims to shorter, easier to understand keys.
            // Like family_name instead of http://schemas.xmlsoap.org/sw/2005/05/identity/claims/surname
            AntiForgeryConfig.UniqueClaimTypeIdentifier = "sub";
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
        }

        public static class InMemoryConfiguration
        {
            public static string IdentityServerURL { get; set; }
            public static string IdentityServerRedirectUrl { get; set; }
        }
    }
}