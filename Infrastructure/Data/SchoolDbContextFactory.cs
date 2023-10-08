using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;
public class SchoolDbContextFactory : IDesignTimeDbContextFactory<SchoolDbContext>
{
    public SchoolDbContext CreateDbContext(string[] args)
    {
        var home = Environment.GetEnvironmentVariable("HOME") ?? "";
        var databasePath = Path.Combine(home, "SchoolDb.sqlite");

        var optionsBuilder = new DbContextOptionsBuilder<SchoolDbContext>();
        optionsBuilder.UseSqlite($"Data Source={databasePath}");

        return new SchoolDbContext(optionsBuilder.Options);
    }
}

