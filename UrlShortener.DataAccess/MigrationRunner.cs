using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UrlShortener.DataAccess;

public static class MigrationRunner
{
    private static string _connectionString;
    
    public static void Initialize(IConfiguration configuration)
    {
        var masterConnectionString = configuration.GetConnectionString("Master");
        var defaultConnectionString = configuration.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(masterConnectionString)) masterConnectionString = defaultConnectionString;
        if (string.IsNullOrWhiteSpace(masterConnectionString))
            throw new System.Exception("Either Master or Default ConnectionString(s) must be provided in configuration");
        
        _connectionString = masterConnectionString;
    }
    
    public static void Run()
    {
        var dataContext = DataContextFactory.Create(_connectionString);
        dataContext.Database.Migrate();
    }
}