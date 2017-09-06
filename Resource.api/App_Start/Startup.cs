using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Thinktecture.IdentityModel.Clients;
using System.Security.Claims;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(Resource.api.App_Start.Startup))]

namespace Resource.api.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            var config = GlobalConfiguration.Configuration;

            //Below example can be used if the issuing authority does not use IdentityServer3

            //var certificate = new X509Certificate2(Convert.FromBase64String("MIIDhDCCAmygAwIBAgIJAN2fE4SEh8gPMA0GCSqGSIb3DQEBCwUAMFcxCzAJBgNVBAYTAlVTMRMwEQYDVQQIDApTb21lLVN0YXRlMSEwHwYDVQQKDBhJbnRlcm5ldCBXaWRnaXRzIFB0eSBMdGQxEDAOBgNVBAMMB2lkcGNlcnQwHhcNMTcwOTA0MTg1NzIwWhcNMTgwOTA0MTg1NzIwWjBXMQswCQYDVQQGEwJVUzETMBEGA1UECAwKU29tZS1TdGF0ZTEhMB8GA1UECgwYSW50ZXJuZXQgV2lkZ2l0cyBQdHkgTHRkMRAwDgYDVQQDDAdpZHBjZXJ0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnGAc8i2b4X2RZG7QToBgzV4WdTUxnjuzADqPqeVZ8aqgczYYJnSTqriMt+n/GGnUiSwiRWvOGdf61eHEOEQjGJrbQr746ymKzVlIWX0NwRXWvX3wpJJUJ8nC4aXoRao77RKotLGVw790ffTksEIaprLSIwjswXnU4db79HK/AxB8avcgXVcljCIH9Awpg6eXYYqlQcK6TNpY5USqiwCmTGlo9kkzeZ1+9hAU/1Bypqxazi2qXgLpA/+iWSOIVn7qjrOSB16ksdMcUEiGFJAkHNWJcfUWlIlW+0xgGBKEOrqTiPLhzhu0EaDvmBIYzVd1at+uqgi6KyBrAZX6+axvLQIDAQABo1MwUTAdBgNVHQ4EFgQUuWmSSya/fcHvDB4rCphl0ALOOEUwHwYDVR0jBBgwFoAUuWmSSya/fcHvDB4rCphl0ALOOEUwDwYDVR0TAQH/BAUwAwEB/zANBgkqhkiG9w0BAQsFAAOCAQEAGy+WezPaxOmWpKDRhCP3OiTb3HahzEFFJ+NQmn7d3zZ+hH3zwDEjkaQsnnqaKrqQmkIIVyTt3i0k7LMEMS7Dz22fq/X/LL96p3jTjZXlNldwM5fBW6Y6irdOE7WqvH7h8CbQ4YKfabxgVLpS+1Z7xBlm01kN15z1gtnlPWOwhcDHSIXzkLtUxK0NlpRubKdvoT1QWwB9rLaZhuuuG5erBK0Xa7kB9ujXDSxnxKDP+boW0enF9O3RAICNluzZb7b+cDQK3CHgj7uOCyllSUlv31B2+tcrd9jANAGh6j8hvwQh/QnmZpe68RrTqdlfQo3ErOco+Jj8T4afsyuXLE62kQ=="));

            //app.UseJwtBearerAuthentication(new Microsoft.Owin.Security.Jwt.JwtBearerAuthenticationOptions
            //{
            //    AllowedAudiences = new[] { "http://localhost:49936" },
            //    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
            //    {
            //        ValidAudience = "http://localhost:49936",
            //        ValidIssuer = "http://localhost:49936",
            //        IssuerSigningKey = new X509SecurityKey(certificate)
            //    }
            //});

            //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            //{
            //    Authority = ConfigurationManager.AppSettings["Authority"]
            //});

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromSeconds(15)
            });

            try
            {
                app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                {
                    ClientId = "idp_code",
                    Authority = ConfigurationManager.AppSettings["Authority"],
                    RedirectUri = "http://localhost:57300/",
                    ResponseType = "code id_token",
                    Scope = "openid profile offline_access",
                    PostLogoutRedirectUri = "http://localhost:57300",
                    SignInAsAuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,

                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthorizationCodeReceived = async notification =>
                        {
                            var requestResponse = await OidcClient.CallTokenEndpointAsync(
                                new Uri("http://localhost:49936/connect/token"),
                                new Uri("http://localhost:57300/"),
                                notification.Code,
                                "idp_code",
                                "secret");

                            var identity = notification.AuthenticationTicket.Identity;

                            identity.AddClaim(new Claim("access_token", requestResponse.AccessToken));
                            identity.AddClaim(new Claim("id_token", requestResponse.IdentityToken));
                            identity.AddClaim(new Claim("refresh_token", requestResponse.RefreshToken));

                            notification.AuthenticationTicket = new AuthenticationTicket(identity, notification.AuthenticationTicket.Properties);
                        }
                    }

                });
            }catch(Exception ex)
            {

            }

            app.UseWebApi(config);
        }
    }
}
