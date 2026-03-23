using Microsoft.EntityFrameworkCore;

namespace UrlShortener.DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Link> Links { get; set; }
}