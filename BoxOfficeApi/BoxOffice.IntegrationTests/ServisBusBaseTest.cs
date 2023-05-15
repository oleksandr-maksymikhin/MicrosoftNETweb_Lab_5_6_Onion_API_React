using BoxOffice.Dal;
using BoxOffice.Platform.BlobStorage;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace BoxOffice.IntegrationTests
{
    public class ServiceBusBaseTest : IDisposable
    {
        protected IHost Host;

        private IHostBuilder _server;
        protected CosmosDbContext CosmosDbContext;
        protected AppDbContext AppDbContext;


        public ServiceBusBaseTest()
        {
        }

        public void Dispose()
        {
            StopServer();
            Host?.Dispose();
            CosmosDbContext?.Dispose();
            AppDbContext?.Dispose();
        }

        public virtual HttpClient GetClient()
        {
            Host = _server.Start();
            CosmosDbContext = Host.Services.GetService(typeof(CosmosDbContext)) as CosmosDbContext;
            AppDbContext = Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;
            return Host.GetTestClient();
        }

        private void StopServer()
        {
            Host?.StopAsync().GetAwaiter().GetResult();
        }

        protected ServiceBusBaseTest InitTestServer()
        {
            _server = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<TestStartup>();
                    //add packet TestDependencyExtention
                    /*webHost.UseTestServer(option =>
                    {
                    option.UseTestEnvironment(
                            services.AddScoped<IBlobStorage, BlobStorage>();
                            );
                    });*/
                });
            return this;
        }
    }
}
