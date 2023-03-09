namespace GraphQL.AutoUnions.Tests
{
    using System;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;
    using global::OneOf;
    using GraphQL.AutoUnions.Tests.Utility;
    using NUnit.Framework;

    public class SubscriptionIntegrationTests
    {
        private class Query
        {
            [GraphQL.Ignore]
            public static readonly BehaviorSubject<Profile> NameSubject
                = new BehaviorSubject<Profile>(
                   new PendingProfile("Name")
                );

            public static string Name => NameSubject.Value
                .Match(
                    p => p.Name,
                    p => p.Name,
                    p => p.Name
                );
        }

        private class Subscription
        {
            public static IObservable<
                Profile
            > UserChanged() => Query.NameSubject;
        }

        [Test]
        public async Task ShouldWork()
        {
            var query = $@"
                subscription {{
                    userChanged {{
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
        }
    }
}