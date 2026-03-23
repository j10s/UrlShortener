using System.Threading.Tasks;
using Reqnroll;

namespace UrlShortener.IntegrationTests.Link.Read;

[Binding]
public sealed class ReadLinkStepDefinitions(
    UrlShortenerClient client,
    ScenarioContext scenarioContext) 
{
    [When(@"a get link request is made with stub {string}")]
    public async Task WhenACreateLinkRequestIsMade(string stub)
    {
        var response = await client.GetLinkAsync(stub);
        scenarioContext.Set(response);
    }
}