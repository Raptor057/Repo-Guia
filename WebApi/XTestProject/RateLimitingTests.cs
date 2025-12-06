using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Project.WebApi.Controllers.Users;

namespace XTestProject;

public class RateLimitingTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RateLimitingTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(_ => { });
    }

    [Fact]
    public async Task HealthEndpoint_ShouldRespondOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GlobalRateLimiter_ShouldReturn429_WhenThresholdExceeded()
    {
        var client = _factory.CreateClient();
        var totalRequests = 120;
        var responses = new List<HttpStatusCode>(totalRequests);

        for (var i = 0; i < totalRequests; i++)
        {
            var res = await client.GetAsync("/api/health");
            responses.Add(res.StatusCode);
        }

        var status429 = responses.Count(code => code == HttpStatusCode.TooManyRequests);
        Assert.True(status429 > 0, "Se esperaban respuestas 429 al exceder el límite global.");
    }
}
