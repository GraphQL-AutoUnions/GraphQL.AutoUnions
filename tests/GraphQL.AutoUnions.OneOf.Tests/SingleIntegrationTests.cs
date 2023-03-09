namespace GraphQL.AutoUnions.Tests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using GraphQL.AutoUnions.Tests.Utility;
    using NUnit.Framework;
    using OneOf;

    public class SingleIntegrationTests
    {
        private class Query
        {
            public static Profile MyProfile()
            {
                return new Random().Next(100) switch
                {
                    < 33 => new PendingProfile("Name"),
                    < 66 => new RegularProfile("Name", "Email"),
                       _ => new PremiumProfile("Name", "Email"),
                };
            }
        }

        [Test]
        public async Task ShouldWork()
        {
            var query = $@"
                    query {{
                        myProfile {{
                            __typename
                            ...on {nameof(PendingProfile)} {{ name }}
                            ...on {nameof(RegularProfile)} {{ name, email }}
                            ...on {nameof(PremiumProfile)} {{ name, email, expirationDate }}
                        }}
                    }}
                ";
            
            var (schema, serializer) = Helpers.Arrange<Query, Mutation, Subscription>();

            var result = await schema.ExecuteAsync(
                serializer,
                options => { options.Query = query; });

            result.Should()
                .NotContain("Exception")
                .And
                .NotContain("Error");
        }
    }
}