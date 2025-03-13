using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Contexts;

namespace SeaEco.EntityFramework.GenericRepository;

public sealed class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<T> _table;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _table = _dbContext.Set<T>();
    }
    
    public async Task<Response> Add(T entity)
    {
        try
        {
            await _table.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return Response.Ok();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }

    public async Task<Response> Update(T entity)
    {
        try
        {
            _table.Update(entity);
            await _dbContext.SaveChangesAsync();
            return Response.Ok();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }

    public async Task<Response> Delete(T entity)
    {
        try
        {
            _table.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return Response.Ok();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }

    public IQueryable<T> GetAll() => _table.AsNoTracking();
    
    public async Task<T?> GetBy(Expression<Func<T, bool>> predicate) => await _table.AsNoTracking().FirstOrDefaultAsync(predicate);
}