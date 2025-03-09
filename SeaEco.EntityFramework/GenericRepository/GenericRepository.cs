using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Contexts;

namespace SeaEco.EntityFramework.GenericRepository;

public sealed class GenericRepository<T>(AppDbContext dbContext) : IGenericRepository<T> where T : class
{
    public async Task<Response> Add(T entity)
    {
        try
        {
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
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
            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();
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
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
            return Response.Ok();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }

    public IQueryable<T> GetAll() => dbContext.Set<T>().AsNoTracking();
}