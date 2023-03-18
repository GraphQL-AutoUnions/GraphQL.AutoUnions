namespace GraphQL.AutoUnions.Tests;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQL.Server.Ui.Playground;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGraphQL(options =>
            {
                options.AddAutoSchema<ProductQuery>();
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
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

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