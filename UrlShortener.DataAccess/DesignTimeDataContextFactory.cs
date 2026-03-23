using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design;

namespace UrlShortener.DataAccess;

[ExcludeFromCodeCoverage(Justification = "Not used in production")]
public class DesignTimeDataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        // Workaround so we can create migrations without having an actual database
        // The connection string is just a dummy
        return DataContextFactory.Create("Host=local;IntegratedSecurity=True");
    }
}