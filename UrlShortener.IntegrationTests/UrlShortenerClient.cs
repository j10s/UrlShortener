using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UrlShortener.Models;

namespace UrlShortener.IntegrationTests;

public class UrlShortenerClient
{
    private readonly HttpClient _httpClient;

    public UrlShortenerClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> HealthCheckAsync()
    {
        return _httpClient.GetAsync("healthz");
    }

    public Task<HttpResponseMessage> CreateLinkAsync(string url)
    {
        var request = new CreateLinkRequest { TargetUri = url };
        return _httpClient.PostAsJsonAsync("link", request);
    }

    public Task<HttpResponseMessage> GetLinkAsync(string stub)
    {
        return _httpClient.GetAsync($"link/{stub}");
    }

    public Task<HttpResponseMessage> UpdateLinkAsync(string stub, string url)
    {
        var request = new UpdateLinkRequest { TargetUri = url };
        return _httpClient.PostAsJsonAsync($"link/{stub}", request);
    }

    public Task<HttpResponseMessage> DeleteLinkAsync(string stub)
    {
        return _httpClient.DeleteAsync($"link/{stub}");
    }
}