using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;

namespace UrlShortener.IntegrationTests.Link.Delete;

[Binding]
public sealed class DeleteLinkStepDefinition(
    UrlShortenerClient client,
    ScenarioContext scenarioContext)
{
    [When("a delete link request is made")]
    public async Task WhenADeleteLinkRequestIsMade()
    {
        var link = scenarioContext.Get<Models.Link>();
        var response = await client.DeleteLinkAsync(link.Stub);
        scenarioContext.Set(response);
    }
    
    [When("a delete link request is made with stub {string}")]
    public async Task WhenADeleteLinkRequestIsMadeWithStub(string stub)
    {
        var response = await client.DeleteLinkAsync(stub);
        scenarioContext.Set(response);
    }

    [Then("the link does not exist")]
    public async Task ThenTheLinkDoesNotExist()
    {
        var link = scenarioContext.Get<Models.Link>();
        var response = await client.GetLinkAsync(link.Stub);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}