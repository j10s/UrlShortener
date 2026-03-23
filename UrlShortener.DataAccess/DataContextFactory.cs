using Microsoft.EntityFrameworkCore;

namespace UrlShortener.DataAccess;

public static class DataContextFactory
{
    public static DataContext Create(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DataContext(optionsBuilder.Options);
    }
}