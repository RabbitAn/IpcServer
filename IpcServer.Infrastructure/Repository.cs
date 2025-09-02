using System.Linq.Expressions;
using IpcServer.Domain.Entities;
using IpcServer.Domain.Interfaces;

namespace IpcServer.Infrastructure;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    protected readonly SqlSugarDbContext _context;

    public Repository(SqlSugarDbContext context)
    {
        _context = context;
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Db.Queryable<T>().InSingleAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Db.Queryable<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Db.Queryable<T>().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Db.Insertable(entity).ExecuteCommandAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        await _context.Db.Updateable(entity).ExecuteCommandAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Db.Deleteable<T>().In(id).ExecuteCommandAsync();
    }
}