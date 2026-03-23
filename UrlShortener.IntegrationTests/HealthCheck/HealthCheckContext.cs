using System.Net;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UrlShortener.IntegrationTests.HealthCheck;

public class HealthCheckContext
{
    public HealthReport Response { get; set; }
        
    public HttpStatusCode StatusCode { get; set; }
}