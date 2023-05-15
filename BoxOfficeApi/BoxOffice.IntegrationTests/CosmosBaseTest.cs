using BoxOffice.Dal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace BoxOffice.IntegrationTests
{
    public class CosmosBaseTest : IDisposable
    {
        protected IHost Host;

        private IHostBuilder _server;
        protected CosmosDbContext CosmosDbContext;

        public CosmosBaseTest()
        {
        }

        public void Dispose()
        {
            StopServer();
            Host?.Dispose();
            CosmosDbContext?.Dispose();
        }

        public virtual HttpClient GetClient()
        {
            Host = _server.Start();
            CosmosDbContext = Host.Services.GetService(typeof(CosmosDbContext)) as CosmosDbContext;
            return Host.GetTestClient();
        }

        private void StopServer()
        {
            Host?.StopAsync().GetAwaiter().GetResult();
        }

        protected CosmosBaseTest InitTestServer()
        {
            _server = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<TestStartup>();
                });
            return this;
        }
    }
}
