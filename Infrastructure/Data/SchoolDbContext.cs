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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(x =>
        {
            x.ToTable("Student").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("StudentID");
            x.Property(p => p.Email);
            x.Property(p => p.Name);
            x.HasMany(p => p.Enrollments).WithOne(p => p.Student);
            //x.Navigation(p => p.Enrollments).AutoInclude();
            x.HasMany(p => p.SportsEnrollments).WithOne(p => p.Student);
            //x.Navigation(p => p.SportsEnrollments).AutoInclude();

            // Seeding student data with realistic information
            x.HasData(
                new Student { Id = 1, Email = "john.doe@email.com", Name = "John Doe" },
                new Student { Id = 2, Email = "jane.smith@email.com", Name = "Jane Smith" },
                new Student { Id = 3, Email = "emily.jones@email.com", Name = "Emily Jones" }
            );
        });
        modelBuilder.Entity<Course>(x =>
        {
            x.ToTable("Course").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("CourseID");
            x.Property(p => p.Name);

            // Seeding course data with realistic information
            x.HasData(
                new Course { Id = 1, Name = "Introduction to Computer Science" },
                new Course { Id = 2, Name = "Data Structures and Algorithms" },
                new Course { Id = 3, Name = "Operating Systems" },
                new Course { Id = 4, Name = "Web Development" },
                new Course { Id = 5, Name = "Machine Learning" }
            );
        });
        modelBuilder.Entity<Enrollment>(x =>
        {
            x.ToTable("Enrollment").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("EnrollmentID");
            x.HasOne(p => p.Student).WithMany(p => p.Enrollments);
            x.Property(p => p.CourseId);
            x.Property(p => p.Grade);
        });
        modelBuilder.Entity<Sports>(x =>
        {
            x.ToTable("Sports").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("SportsID");
            x.Property(p => p.Name);
        });
        modelBuilder.Entity<SportsEnrollment>(x =>
        {
            x.ToTable("SportsEnrollment").HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName("SportsEnrollmentID");
            x.HasOne(p => p.Student).WithMany(p => p.SportsEnrollments);
            x.Property(p => p.SportsId);
            x.Property(p => p.Grade);
        });
        modelBuilder.Entity<EnrollmentData>(x =>
        {
            x.HasNoKey();
            x.Property(p => p.StudentId);
            x.Property(p => p.Grade);
            x.Property(p => p.Course);
        });
    }
}
