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
                        new Claim(Constants.ClaimTypes.Name, "Austin Lynn")
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
                    ClientId = "IDP",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientName = "IDP",
                    Flow = Flows.ResourceOwner,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        "read"
                    },
                    Enabled = true
                }
            };
        }
    }
}