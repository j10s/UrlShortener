using Microsoft.AspNetCore.Mvc.Testing;
using Reqnroll;
using Reqnroll.BoDi;
using UrlShortener.WebApi;

namespace UrlShortener.IntegrationTests;

[Binding]
public class SystemUnderTest
{
    [BeforeTestRun]
    public static void CreateWebApi(IObjectContainer container)
    {
        var factory = new WebApplicationFactory<Startup>();
        var httpClient = factory.CreateClient();

        container.RegisterInstanceAs(httpClient);
    } 
}