using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using Microsoft.Ajax.Utilities;
using ScopeType = IdentityServer3.Core.Models.ScopeType;

namespace Identifix.IdentityServer
{
    public static class IdentityFactory
    {
        public static IEnumerable<Client> GetClients()
        {
            string redirectURL = ConfigurationManager.AppSettings["RedirectURL"];
            redirectURL = string.IsNullOrEmpty(redirectURL) ? "https://localhost:44301/" : redirectURL;

            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "IDX Corporate",
                    ClientId = "idc",
                    Flow = Flows.Implicit,

                    
                    RedirectUris = new List<string>
                    {
                        redirectURL
                    },

                    AllowAccessToAllScopes = true
                }
            };
        }

        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "lhouse01",
                    Password = "xl97acc2000",
                    Subject = "1",

                    Claims = new[]
                    {
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Larry"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "House"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "DHUser"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "IDXShopper")
                    }
                },
                new InMemoryUser
                {
                    Username = "memo0001",
                    Password = "password",
                    Subject = "2",

                    Claims = new[]
                    {
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Mike"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "Emo"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "DHUser"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "IDXShopper")
                    }
                },
                 new InMemoryUser
                {
                    Username = "mrob1",
                    Password = "password",
                    Subject = "3",

                    Claims = new[]
                    {
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Marty"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "Robertson"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "DHUser"),
                        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "IDXShopper")
                    }
                }
            };
        }

        public static IEnumerable<Scope> GetScopes()
        {
            var scopes = new List<Scope>
            {
                new Scope
                {
                    Enabled = true,
                    Name = "roles",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                }
            };

            scopes.AddRange(StandardScopes.All);

            return scopes;

        }
    }
}