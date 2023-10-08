using Application.Interfaces;
using Domain;
using Infrastructure.Data;

namespace Infrastructure.Repositories;
public class StudentRepository : IRepository<Student>
{
    private readonly SchoolDbContext _context;

    public StudentRepository(SchoolDbContext context)
    {
        _context = context;
    }
    public IReadOnlyList<Student> GetAll()
    {
        return _context.Students.ToList();
    }

    public Task<Student> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Student> AddAsync(Student entity)
    {
        _context.Students.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public Task<Student> UpdateAsync(Student entity)
    {
        throw new NotImplementedException();
    }

    public Task<Student> DeleteAsync(Student entity)
    {
        throw new NotImplementedException();
    }
}
