using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;
using Autofac;
using IdentityServer3.Core.Configuration;
using Owin;
using Identifix.IdentityServer;
using Identifix.IdentityServer.App_Start;
using Identifix.IdentityServer.Models;
using Identifix.IdentityServer.Models.Data;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IUserService = IdentityServer3.Core.Services.IUserService;
using IdentityServer3.Core.Logging;

namespace Identifix.IdentityServer.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IUserService userService = DependencyConfig.Instance.Resolve<IUserService>();
            var registration = new IdentityServer3.Core.Configuration.Registration<IUserService>(userService);

            //EventsOptions eventOptions = new EventsOptions() { RaiseSuccessEvents = true };
            LoggingOptions logOptions = new LoggingOptions();

            var options = new AuthenticationOptions();
            options.LoginPageLinks = new List<LoginPageLink>() { new LoginPageLink() { Text = "Forgot your password?", Href = "/User/Default.html#/generateResetPasswordLink" } };

            // Web API internal diagnostic logging will be forwarded to the log provider
            logOptions.EnableWebApiDiagnostics = true;

            // If enabled, HTTP requests and responses will be logged
            logOptions.EnableHttpLogging = false;
            
            app.Map("/identity", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Identifix",
                    SigningCertificate = LoadCertificate(),

                    Factory = new IdentityServerServiceFactory() { UserService = registration }
                                .UseInMemoryClients(IdentityFactory.GetClients())
                                .UseInMemoryScopes(IdentityFactory.GetScopes()),
                    LoggingOptions = logOptions,
                    AuthenticationOptions = options
                });

            });            

            

            AntiForgeryConfig.UniqueClaimTypeIdentifier = IdentityServer3.Core.Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            Database.SetInitializer(new UserDatabaseInitializer());

            AutoMapperConfiguration.DefineAutoMapConfiguration();
        }

        X509Certificate2 LoadCertificate()
        {
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory}bin\idsrv3test.pfx";
            return new X509Certificate2( path, "idsrv3test");
        }

    }
}