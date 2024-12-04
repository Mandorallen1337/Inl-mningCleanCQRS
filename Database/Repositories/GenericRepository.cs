using Database.Databases;
using Database.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly RealDatabase _realDatabase;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(RealDatabase realDatabase)
    {
        _realDatabase = realDatabase;
        _dbSet = realDatabase.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

        try
        {
            _dbSet.Attach(entity);
            await _dbSet.AddAsync(entity);
            await _realDatabase.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryOperationException("An error occurred while adding the entity.", ex);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

        try
        {
            _dbSet.Remove(entity);
            await _realDatabase.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryOperationException("An error occurred while deleting the entity.", ex);
        }
    }

    public async Task<T?> FindByAsync(Expression <Func<T, bool>> predicate)
    {
        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null.");
        }
        try
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        catch (Exception exception)
        {
            throw new RepositoryOperationException("An error occurred while finding the entity.", exception);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("An error occurred while retrieving entities.", ex);
        }
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("The ID cannot be empty.", nameof(id));

        try
        {
            var entity = await _dbSet.FindAsync(id);
            return entity ?? throw new KeyNotFoundException($"Entity with ID {id} was not found.");
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("An error occurred while retrieving the entity.", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

        try
        {
            _dbSet.Update(entity);
            await _realDatabase.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryOperationException("An error occurred while updating the entity.", ex);
        }
    }
}
