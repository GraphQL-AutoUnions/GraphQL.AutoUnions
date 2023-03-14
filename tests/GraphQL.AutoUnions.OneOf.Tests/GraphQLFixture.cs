namespace GraphQL.AutoUnions.Tests;

using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class GraphQLFixture : IDisposable
{
    private readonly TestServer _server;

    public GraphQLFixture()
    {
        _server = new TestServer(
            new WebHostBuilder()
                .UseStartup<Startup>());
        
        Client = _server.CreateClient();
    }

    public HttpClient Client { get; }

    public void Dispose()
    {
        Client.Dispose();
        _server.Dispose();
    }
}