using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;

namespace UrlShortener.IntegrationTests.Link.Update;

[Binding]
public class UpdateLinkStepDefinitions(
    UrlShortenerClient client,
    ScenarioContext scenarioContext)
{
    [When("an update link request is made with url {string}")]
    public async Task WhenAnUpdateLinkRequestIsMadeWithUrl(string url)
    {
        var link = scenarioContext.Get<Models.Link>();
        var response = await client.UpdateLinkAsync(link.Stub, url);
        scenarioContext.Set(response);
    }
    
    [When(@"an update link request is made with stub {string} and url {string}")]
    public async Task WhenAnUpdateLinkRequestIsMade(string stub, string url)
    {
        var response = await client.UpdateLinkAsync(stub, url);
        scenarioContext.Set(response);
    }

    [Then("the link has url {string}")]
    public async Task ThenTheLinkHasUrl(string url)
    {
        var link = await scenarioContext.Get<HttpResponseMessage>().Content.ReadFromJsonAsync<Models.Link>();
        link.TargetUri.Should().Be(url);
    }
}