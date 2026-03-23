using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;

namespace UrlShortener.IntegrationTests.Link.Create;

[Binding]
public sealed class CreateLinkStepDefinitions(
    UrlShortenerClient client,
    CreateLinkContext context,
    ScenarioContext scenarioContext)
{
    [Given(@"a url of {string}")]
    public void GivenAUrlOf(string url)
    {
        context.Url = url;
    }

    [When(@"a create link request is made")]
    public async Task WhenACreateLinkRequestIsMade()
    {
        var response = await client.CreateLinkAsync(context.Url);
        scenarioContext.Set(response);
    }

    [Then(@"a link exists with url {string}")]
    public async Task ThenALinkExistsWithUrl(string url)
    {
        var createResponse = await scenarioContext.Get<HttpResponseMessage>().Content.ReadFromJsonAsync<Models.Link>();
        var getResponse = await client.GetLinkAsync(createResponse.Stub);
        var link = await getResponse.Content.ReadFromJsonAsync<Models.Link>();
        
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        link.TargetUri.Should().Be(url);
    }
}