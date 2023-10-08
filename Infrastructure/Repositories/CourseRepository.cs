using Application.Interfaces;
using Domain;
using Infrastructure.Data;

namespace Infrastructure.Repositories;
public class CourseRepository : IRepository<Course>
{
    private readonly SchoolDbContext _context;
    public CourseRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Course> GetAll()
    {
        return _context.Courses.ToList();
    }

    public Task<Course> GetByIdAsync(long id)
    {
        var course = _context.Courses.Find(id);
        if (course == null)
        {
            throw new Exception($"Course with id {id} not found");
        }
        return Task.FromResult(course);
    }

    public Task<Course> AddAsync(Course entity)
    {
        _context.Courses.Add(entity);
        _context.SaveChangesAsync();
        return Task.FromResult(entity);
    }

    public Task<Course> UpdateAsync(Course entity)
    {
        throw new NotImplementedException();
    }

    public Task<Course> DeleteAsync(Course entity)
    {
        throw new NotImplementedException();
    }
}
