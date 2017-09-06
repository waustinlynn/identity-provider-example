using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Services.InMemory;
using System.Security.Claims;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;

namespace IDP
{
    public class InMemoryManager
    {
        public List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "austin.lynn",
                    Username = "austin.lynn",
                    Password = "mypass",
                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.Name, "Austin Lynn"),
                        new Claim("custom_type", "custom_value")
                    }
                }
            };
        }

        public IEnumerable<Scope> GetScopes()
        {
            return new[]
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess
            };
        }

        public IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "idp_code",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientName = "idp",
                    Flow = Flows.Hybrid,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.OfflineAccess,
                        "read"
                    },
                    RedirectUris = new List<string>
                    {
                        "http://localhost:57300/",
                        "http://localhost:4200"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:57300"
                    },
                    Enabled = true
                },
                new Client
                {
                    ClientId = "idp_implicit",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientName = "idp",
                    Flow = Flows.Implicit,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        "read"
                    },
                    RedirectUris = new List<string>
                    {
                        "http://localhost:4200/",
                        "http://localhost:4200"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:4200"
                    },
                    Enabled = true
                }
            };
        }
    }
}