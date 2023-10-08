namespace Application.Interfaces;
public interface IRepository<T> where T : class
{
    IReadOnlyList<T> GetAll();
    Task<T> GetByIdAsync(long id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}
