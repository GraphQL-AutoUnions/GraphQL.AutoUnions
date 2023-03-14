using Xunit;

namespace GraphQL.AutoUnions.Tests;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class GraphQLTests : IClassFixture<GraphQLFixture>
{
    private readonly HttpClient _client;
    private readonly static string _testDataPath = "TestData";

    public GraphQLTests(GraphQLFixture fixture)
    {
        _client = fixture.Client;
    }

    [Theory]
    [MemberData(nameof(GetTestCases))]
    public async Task TestGraphQL(object query, HttpStatusCode expectedStatusCode, string expectedBody)
    {
        var response = await _client.PostAsync("/graphql", new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json"));
        var actualStatusCode = response.StatusCode;
        var actualBody = await response.Content.ReadAsStringAsync();
        
        var expectedBodyObject = JsonConvert.DeserializeObject(expectedBody);
        var actualBodyObject = JsonConvert.DeserializeObject(actualBody);

        Assert.Equal(expectedStatusCode, actualStatusCode);
        Assert.Equal(actualBodyObject, expectedBodyObject);
    }

    public static IEnumerable<object[]> GetTestCases()
    {
        var directoryInfo = new DirectoryInfo(_testDataPath);
        var testFiles = directoryInfo.GetFiles("*.gql", SearchOption.AllDirectories);
        return testFiles.Select(file =>
        {
            var query = new { query = File.ReadAllText(file.FullName)};
            var statusCode = HttpStatusCode.OK;
            var expectedBody = File.ReadAllText(file.FullName + ".json");
            return new object[] { query, statusCode, expectedBody };
        });
    }
}