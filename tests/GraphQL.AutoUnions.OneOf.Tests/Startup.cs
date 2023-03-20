namespace GraphQL.AutoUnions.Tests;

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.WebSockets;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddHttpLogging(options =>
        {
        });
        
        services.AddWebSockets(options =>
        {
            options.AllowedOrigins.Add("*");
        });

        services.AddGraphQL(options =>
            {
                options.AddAutoSchema<ProductQuery>((configureAutoSchema) =>
                    configureAutoSchema
                        .WithMutation<ProductMutation>()
                        .WithSubscription<ProductSubscription>());
                options.AddOneOfAutoUnions();
                options.AddSystemTextJson();
                options.AddErrorInfoProvider((error) =>
                {
                    error.ExposeExceptionDetails = true;
                });
            });

        services.AddRouting();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpLogging();
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseWebSockets();
        app.UseRouting();

        app.UseGraphQL("/graphql", (options) =>
        {
            options.HandleGet = false;
            options.HandleWebSockets = true;
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL();
        });

        app.UseGraphQLPlayground("/ui/playground", new PlaygroundOptions()
        {
            GraphQLEndPoint = "/graphql"
        });
    }
}