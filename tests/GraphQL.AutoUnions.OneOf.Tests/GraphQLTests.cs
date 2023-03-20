using Xunit;

namespace GraphQL.AutoUnions.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using Newtonsoft.Json;

public class GraphQLTests : IClassFixture<GraphQLFixture>
{
    private readonly GraphQLHttpClient _client;
    private readonly static string _testDataPath = "TestData";

    public GraphQLTests(GraphQLFixture fixture)
    {
        this._client = fixture.Client;
    }

    [Theory]
    [MemberData(nameof(GetTestCaseNames), parameters: new object[] { "Queries/*.gql" })]
    public Task Queries(string testName)
    {
        return this.TestGraphQL(testName, "Queries/*.gql", 1);
    }

    [Theory]
    [MemberData(nameof(GetTestCaseNames), parameters: new object[] { "Mutations/*.gql" })]
    public Task Mutations(string testName)
    {
        return this.TestGraphQL(testName, "Mutations/*.gql", 2);
    }

    [Theory]
    [MemberData(nameof(GetTestCaseNames), parameters: new object[] { "Subscriptions/*.gql" })]
    public Task Subscriptions(string testName)
    {
        return this.TestGraphQL(testName, "Subscriptions/*.gql", 3);
    }

    private async Task TestGraphQL(string testName, string pattern, int type)
    {
        var (query, expectedResponse) = this.GetTestCase(testName, pattern);
        var response =
            type switch
            {
                1 => await this.QueryAsync(query),
                2 => await this.MutationAsync(query),
                3 => await this.SubscriptionAsync(query),
            };
        
        var expectedResponseObject = JsonConvert.DeserializeObject(expectedResponse);
        var responseObject = JsonConvert.DeserializeObject(response);

        response = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
        expectedResponse = JsonConvert.SerializeObject(expectedResponseObject, Formatting.Indented);

        response.Should().Be(expectedResponse);
    }

    private async Task<string> SubscriptionAsync(string query)
    {
        var subscriptionStream = this._client.CreateSubscriptionStream<object>(new GraphQLRequest()
        {
            Query = query
        });

        var list = await subscriptionStream
            .Scan(new List<object>(), (acc, curr) =>
            {
                acc.Add(curr);

                return acc;
            })
            .ToTask();
        
        return JsonConvert.SerializeObject(list);
    }

    private async Task<string> QueryAsync(string query)
    {
        var request = new GraphQLRequest { Query = query };
        var response = await this._client.SendQueryAsync<object>(request) as GraphQLHttpResponse<object>;

        return JsonConvert.SerializeObject(new { statusCode = response!.StatusCode, data = response.Data, errors = response.Errors });
    }
    
    private async Task<string> MutationAsync(string query)
    {
        var request = new GraphQLRequest { Query = query };
        var response = await this._client.SendMutationAsync<object>(request) as GraphQLHttpResponse<object>;

        return JsonConvert.SerializeObject(new { statusCode = response!.StatusCode, data = response.Data, errors = response.Errors });
    }

    public (string, string) GetTestCase(string name, string pattern)
    {
        var directoryInfo = new DirectoryInfo(_testDataPath);
        var testFiles = directoryInfo.GetFiles(pattern, SearchOption.AllDirectories);

        var file = testFiles.Where(file =>
        {
            var testName = Path.GetFileNameWithoutExtension(file.FullName);

            return testName == name;
        }).SingleOrDefault();

        var query = File.ReadAllText(file.FullName);
        var expectedBody = File.ReadAllText(file.FullName + ".json");
        return (query, expectedBody);
    }

    public static IEnumerable<object[]> GetTestCaseNames(string pattern)
    {
        var directoryInfo = new DirectoryInfo(_testDataPath);
        var testFiles = directoryInfo.GetFiles(pattern, SearchOption.AllDirectories);

        return testFiles.Select(file =>
        {
            var testName = Path.GetFileNameWithoutExtension(file.FullName);

            return new object[] { testName };
        });
    }
}