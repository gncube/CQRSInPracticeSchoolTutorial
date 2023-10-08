using Application.Interfaces;
using Domain;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Infrastructure;
public static class RegisterDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var home = Environment.GetEnvironmentVariable("HOME") ?? "";
        var databasePath = Path.Combine(home, "SchoolDb.sqlite");

        services.AddDbContext<SchoolDbContext>(options =>
        {
            options.UseSqlite($"Data Source={databasePath}");
        });

        // Add custom JsonSerializerOptions
        services.AddSingleton(x => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Add SubscriberRepository
        services.AddScoped<IRepository<Student>, StudentRepository>();
        services.AddScoped<IRepository<Course>, CourseRepository>();
        return services;
    }
}
