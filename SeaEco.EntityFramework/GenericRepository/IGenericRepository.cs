using SeaEco.Abstractions.ResponseService;

namespace SeaEco.EntityFramework.GenericRepository;

public interface IGenericRepository<T> where T : class
{
    Task<Response> Add(T entity);
    Task<Response> Update(T entity);
    Task<Response> Delete(T entity);

    IQueryable<T> GetAll();
}