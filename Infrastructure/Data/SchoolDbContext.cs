using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; } = default!;
    public DbSet<Course> Courses { get; set; } = default!;
    public DbSet<Sports> Sports { get; set; } = default!;
    public DbSet<SportsEnrollment> SportsEnrollments { get; set; } = default!;

}
