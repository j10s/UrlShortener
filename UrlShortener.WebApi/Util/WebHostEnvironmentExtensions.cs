using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace UrlShortener.WebApi.Util;

public static class WebHostEnvironmentExtensions
{
    public static bool IsDevelopmentOrIntegration(this IWebHostEnvironment env)
    {
        return env.IsDevelopment() || env.IsEnvironment("Integration");
    }
}