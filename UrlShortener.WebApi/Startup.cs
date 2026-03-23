using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sqids;
using StackExchange.Redis.Extensions.Core.Configuration;
using UrlShortener.DataAccess;
using UrlShortener.WebApi.Link;
using UrlShortener.WebApi.Util;

namespace UrlShortener.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllers();
        services.AddSwaggerGen();

        var connectionString = Configuration.GetConnectionString("Default");
        var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();

        services.AddDbContextPool<DataContext>(builder => builder.UseNpgsql(connectionString));
        services.AddSingleton<IMemoryCacheWrapper, MemoryCacheWrapper>();
        services.AddScoped<ILinkRepository, EntityFrameworkLinkRepository>();
        services.AddCaching(Configuration);
        services.AddSingleton(new SqidsEncoder<long>());
        services.AddScoped<ILinkService, LinkService>();

        services.AddHealthChecks()
            .AddNpgSql(connectionString, name: "Postgres")
            .AddDbContextCheck<DataContext>("Entity Framework")
            .AddRedis(redisConfiguration.ConfigurationOptions.ToString(), "Redis");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopmentOrIntegration())
        {
            app.UseDeveloperExceptionPage();
            MigrationRunner.Initialize(Configuration);
            MigrationRunner.Run();
        }
        
        app.UseSerilogRequestLogging();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        });
    }
}