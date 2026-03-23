using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace UrlShortener.WebApi;

public class Program
{
    [ExcludeFromCodeCoverage(Justification = "Untestable")]
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, services, loggerConfiguration) =>
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}