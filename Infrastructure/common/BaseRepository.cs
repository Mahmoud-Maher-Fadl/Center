using System.Data;
using Core.common;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.common;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext Context;

    public BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }


    public async Task<T> Add(T entity)
    {
        var result = await Context.Set<T>().AddAsync(entity);
        //await Context.SaveChangesAsync();
        return result.Entity;
    }

    public Task<T> Update(T entity)
    {
        var result = Context.Set<T>().Update(entity);
        // await Context.SaveChangesAsync();
        return Task.FromResult(result.Entity);
    }

    public Task<int> UpdateRange(List<T> entities)
    {
        Context.Set<T>().UpdateRange(entities);
        return Task.FromResult(entities.Count);
    }

    /*public async Task<List<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetById(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity is null)
            return (T)(object)$"{typeof(T).Name} with ID {id} not found";
        return entity;
    }*/


    public async Task<T> DeleteById(int id)
    {
        var entity = await Context.Set<T>().FindAsync(id);
        if (entity is null)
            return (T)(object)$"{typeof(T).Name} with ID {id} not found";
        var result = Context.Set<T>().Remove(entity);
        //await Context.SaveChangesAsync();
        return result.Entity;
    }


    public Task<int> DeleteRange(List<T> entities)
    {
        entities = new List<T>();
        Context.Set<T>().RemoveRange(entities);
        //await Context.SaveChangesAsync();
        return Task.FromResult(entities.Count);
    }
}