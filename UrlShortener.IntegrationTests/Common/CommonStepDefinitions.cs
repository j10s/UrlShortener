using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Reqnroll;

namespace UrlShortener.IntegrationTests.Common;

[Binding]
public class CommonStepDefinitions(
    UrlShortenerClient client,
    ScenarioContext scenarioContext)
{
    [Given(@"a link exists with url {string}")]
    public async Task GivenALinkExistsWithUrl(string url)
    {
        var response = await client.CreateLinkAsync(url);
        var link = await response.Content.ReadFromJsonAsync<Models.Link>();
        scenarioContext.Set(link);
    }
    
    [Then(@"the http status code is {int}")]
    public void ThenTheHttpStatusCodeIs(int statusCode)
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        response.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }
    
    [Then(@"the following errors are returned:")]
    public async Task ThenTheFollowingErrorsAreReturned(DataTable table)
    {
        var tableErrors = table.CreateSet<Error>();
        var response = scenarioContext.Get<HttpResponseMessage>();
        var validationProblemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        var validationErrors = validationProblemDetails.Errors;

        foreach (var error in tableErrors)
        {
           validationErrors[error.Field].Should().Contain(errorMessage =>  errorMessage == error.ErrorMessage);
        }        
    }
}