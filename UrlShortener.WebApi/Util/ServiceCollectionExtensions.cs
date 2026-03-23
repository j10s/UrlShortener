using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;
using UrlShortener.WebApi.Link;

namespace UrlShortener.WebApi.Util;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetSection("Redis").Get<ExtendedRedisConfiguration>();
        var memoryCacheConfiguration = configuration.GetSection("MemoryCache").Get<MemoryCacheConfiguration>();

        if (redisConfiguration.Enabled)
        {
            services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(redisConfiguration);
            services.Decorate<ILinkRepository, RedisLinkRepository>();   
        }

        if (memoryCacheConfiguration.Enabled)
        {
            services.AddSingleton<IMemoryCache>(new MemoryCache(memoryCacheConfiguration));
            services.Decorate<ILinkRepository, InMemoryLinkRepository>();
        }
        
        return services;
    }

    public class ExtendedRedisConfiguration : RedisConfiguration
    {
        public bool Enabled { get; set; }
    }
    
    public class MemoryCacheConfiguration : MemoryCacheOptions
    {
        public bool Enabled { get; set; }
    }
}