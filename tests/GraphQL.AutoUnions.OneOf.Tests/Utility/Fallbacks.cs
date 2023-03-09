namespace GraphQL.AutoUnions.Tests.Utility;

using System;

internal class Mutation
{
    public static string Noop()
    {
        throw new NotImplementedException();
    }
}

internal class Subscription
{
    public static IObservable<string> Noop()
    {
        throw new NotImplementedException();
    }
}