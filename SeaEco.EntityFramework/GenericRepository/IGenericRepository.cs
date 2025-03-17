using System.Linq.Expressions;
using SeaEco.Abstractions.ResponseService;

namespace SeaEco.EntityFramework.GenericRepository;

public interface IGenericRepository<T> where T : class
{
    Task<Response> Add(T entity);
    Task<Response> Update(T entity);
    Task<Response> UpdateRange(IEnumerable<T> entities);
    Task<Response> Delete(T entity);

    IQueryable<T> GetAll();
    Task<T?> GetBy(Expression<Func<T, bool>> predicate);
}