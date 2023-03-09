namespace GraphQL.AutoUnions.Tests.Utility;

using System;
using OneOf;

public record PendingProfile(string Name);
public record RegularProfile(string Name, string Email);
public record PremiumProfile(string Name, string Email)
{
    public static DateTime ExpirationDate => DateTime.Now + TimeSpan.FromDays(30); 
}

[GenerateOneOf]
public partial class Profile : OneOfBase<
    PendingProfile,
    RegularProfile,
    PremiumProfile
>
{
}