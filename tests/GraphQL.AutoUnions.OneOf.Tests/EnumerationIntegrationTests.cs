namespace GraphQL.AutoUnions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using GraphQL.AutoUnions.Tests.Utility;
    using NUnit.Framework;

    public class EnumerationIntegrationTests
    {
        private class Query
        {
            public static IEnumerable<Profile> MyProfiles()
            {
                for (int i = 0; i < 10; i++)
                {
                    yield return new Random().Next(100) switch
                    {
                        < 33 => new PendingProfile("Name"),
                        < 66 => new RegularProfile("Name", "Email"),
                        _ => new PremiumProfile("Name", "Email"),
                    };
                }
            }
        }

        [Test]
        public async Task ShouldWork()
        {
            var query = $@"
                query {{
                    myProfiles {{
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