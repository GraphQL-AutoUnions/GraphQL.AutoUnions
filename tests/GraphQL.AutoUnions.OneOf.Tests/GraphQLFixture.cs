namespace GraphQL.AutoUnions.Tests;

using System;
using System.Net.Http;
using System.Threading;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

public class GraphQLFixture : IDisposable
{
    private readonly IWebHost app;

    public GraphQLFixture()
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection();
        var config = configBuilder.Build();
        config["server.urls"] = "http://localhost:54321";

        var builder  = new WebHostBuilder()
            .UseConfiguration(config)
            .UseKestrel()
            .UseStartup<Startup>();
        
        var graphQLEndpoint = "http://localhost:54321/graphql";
        var webSocketEndpoint = "ws://localhost:54321/graphql";

        this.app = builder.Build();

        _ = app.StartAsync();
        
        var options = new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(graphQLEndpoint),
            WebSocketEndPoint = new Uri(webSocketEndpoint),
        };
        
        this.Client = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer());
    }

    public GraphQLHttpClient Client { get; }

    public void Dispose()
    {
        this.Client.Dispose();
        this.app.Dispose();
    }
}