using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Reqnroll;

namespace UrlShortener.IntegrationTests.HealthCheck;

[Binding]
public sealed class HealthCheckStepDefinitions(
    UrlShortenerClient client,
    HealthCheckContext context,
    ScenarioContext scenarioContext)
{
    [When("a health check request is made")]
    public async Task WhenAHealthCheckRequestIsMade()
    {
        var response = await client.HealthCheckAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var healthReportResponse = await response.Content.ReadFromJsonAsync<SerializableHealthReport>(options);
        context.Response = healthReportResponse.ToHealthReport();
        context.StatusCode = response.StatusCode;
        scenarioContext.Set(response);
    }

    [Then("a healthy response is returned")]
    public void ThenAHealthyResponseIsReturned()
    {
        context.StatusCode.Should().Be(HttpStatusCode.OK);
        context.Response.Status.Should().Be(HealthStatus.Healthy);
        context.Response.Entries.Should().OnlyContain(kv => kv.Value.Status == HealthStatus.Healthy);
    }
}