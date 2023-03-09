namespace GraphQL.AutoUnions.Tests.Utility;

using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using OneOf;

public static class Helpers
{
    public static (ISchema, IGraphQLTextSerializer) Arrange<Q, M, S>()
    {
        // create a new instance of the service collection
        IServiceCollection services = new ServiceCollection();
            
        // register the services that your application uses
        services.AddGraphQL((gqlBuilder) =>
        {
            gqlBuilder.AddOneOfAutoUnions();
            gqlBuilder.AddAutoSchema<Q>(configureAutoSchema =>
            {
                configureAutoSchema
                    .WithMutation<M>()
                    .WithSubscription<S>();
            });
            gqlBuilder.AddSystemTextJson();
            gqlBuilder.AddErrorInfoProvider((options) =>
            {
                options.ExposeData = true;
                options.ExposeCode = true;
                options.ExposeCodes = true;
                options.ExposeExceptionStackTrace = true;
            });
        });

        // build the service provider
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var autoSchema = serviceProvider.GetRequiredService<AutoSchema<Q>>();
            
        autoSchema.Initialize();
            
        return (autoSchema, serviceProvider.GetRequiredService<IGraphQLTextSerializer>());
    }
}