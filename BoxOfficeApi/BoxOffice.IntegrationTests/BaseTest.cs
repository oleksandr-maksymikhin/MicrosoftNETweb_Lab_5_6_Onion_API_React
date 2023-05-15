using BoxOffice.Dal;
using BoxOffice.IntegrationTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace BoxOffice.IntegrationTests;

public class BaseTest : IDisposable
{
    protected IHost Host;

    private IHostBuilder _server;
    protected AppDbContext AppDbContext;
    public BaseTest()
    {
    }

    public void Dispose()
    {
        StopServer();
        Host?.Dispose();
        AppDbContext?.Dispose();
    }

    public virtual HttpClient GetClient()
    {
        Host = _server.Start();
        AppDbContext = Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;
        return Host.GetTestClient();
    }

    private void StopServer()
    {
        Host?.StopAsync().GetAwaiter().GetResult();
    }

    protected BaseTest InitTestServer()
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
