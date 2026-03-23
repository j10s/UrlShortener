using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System.IO;
using Reqnroll;

namespace UrlShortener.IntegrationTests;

[Binding]
public class Docker
{
    private static ICompositeService _compose;

    [BeforeTestRun(Order = 0)]
    public static void StartContainers()
    {
        var filePathBase = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
        var composeFile = Path.Combine(filePathBase, "docker-compose.integration.yml");

        _compose = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(composeFile)
            .Build()
            .Start();
    }

    [AfterTestRun]
    public static void StopContainers()
    {
        _compose.Stop();
        _compose.Dispose();
    }
}