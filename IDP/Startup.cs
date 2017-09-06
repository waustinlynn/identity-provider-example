using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

[assembly: OwinStartup(typeof(IDP.Startup))]

namespace IDP
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //Log.Logger = new LoggerConfiguration()
            var inMemoryManager = new InMemoryManager();
            var factory = new IdentityServerServiceFactory().UseInMemoryUsers(inMemoryManager.GetUsers()).UseInMemoryScopes(inMemoryManager.GetScopes()).UseInMemoryClients(inMemoryManager.GetClients());
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);


            var certificatePath = ConfigurationManager.AppSettings["SecurityKeyPath"];

            var pass = ConfigurationManager.AppSettings["SecurityKeyPassword"];
            var x509Cert = new X509Certificate2(certificatePath, pass);

            var options = new IdentityServerOptions
            {
                SigningCertificate = x509Cert,
                RequireSsl = false,
                Factory = factory
            };

            

            app.UseIdentityServer(options);
        }
        
    }
}
