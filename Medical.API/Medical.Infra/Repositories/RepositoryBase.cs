namespace Medical.Infra.Repositories;
public class RepositoryBase<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly ILogger<RepositoryBase<T>> _logger;

    public RepositoryBase(ApplicationDbContext context, ILogger<RepositoryBase<T>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        try 
        {
            // Log entity tracking state before retrieval
            var trackedEntities = _context.ChangeTracker.Entries<T>()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            _logger.LogInformation($"Tracked {typeof(T).Name} entities before GetByIdAsync: {trackedEntities.Count}");

            // Detach any existing tracked entity with the same key to prevent conflicts
            var existingEntry = trackedEntities
                .FirstOrDefault(e => e.Property("Id").CurrentValue.Equals(id));
            
            if (existingEntry != null)
            {
                _logger.LogWarning($"Detaching existing {typeof(T).Name} entity with ID {id} to prevent tracking conflict");
                _context.Entry(existingEntry.Entity).State = EntityState.Detached;
            }

            var entity = await _context.Set<T>().FindAsync(id);
            
            if (entity == null)
            {
                _logger.LogWarning($"{typeof(T).Name} with ID {id} not found");
            }

            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving {typeof(T).Name} with ID {id}");
            throw;
        }
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync()
    {
        try 
        {
            _logger.LogInformation($"Clearing change tracker before retrieving all {typeof(T).Name} entities");
            
            // Clear change tracker to prevent tracking conflicts
            _context.ChangeTracker.Clear();
            
            var entities = await _context.Set<T>().ToListAsync();
            
            _logger.LogInformation($"Retrieved {entities.Count} {typeof(T).Name} entities");
            
            return entities;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving all {typeof(T).Name} entities");
            throw;
        }
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        try 
        {
            // Log current tracked entities before adding
            var trackedEntities = _context.ChangeTracker.Entries<T>()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            _logger.LogInformation($"Tracked {typeof(T).Name} entities before AddAsync: {trackedEntities.Count}");

            // Ensure no existing entity with the same key is being tracked
            var existingEntry = trackedEntities
                .FirstOrDefault(e => e.Property("Id").CurrentValue.Equals(((dynamic)entity).Id));
            
            if (existingEntry != null)
            {
                _logger.LogWarning($"Detaching existing {typeof(T).Name} entity with ID {((dynamic)entity).Id} before adding");
                _context.Entry(existingEntry.Entity).State = EntityState.Detached;
            }

            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Added new {typeof(T).Name} with ID {((dynamic)entity).Id}");
            
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding {typeof(T).Name} entity");
            throw;
        }
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        try 
        {
            _logger.LogInformation($"Updating {typeof(T).Name} entity");
            
            // Log current tracked entities
            var trackedEntities = _context.ChangeTracker.Entries<T>()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            _logger.LogInformation($"Tracked {typeof(T).Name} entities before UpdateAsync: {trackedEntities.Count}");

            // Detach any existing tracked entities to prevent conflicts
            _context.ChangeTracker.Clear();

            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Updated {typeof(T).Name} with ID {((dynamic)entity).Id}");
            
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating {typeof(T).Name} entity");
            throw;
        }
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        try 
        {
            _logger.LogInformation($"Deleting {typeof(T).Name} with ID {id}");
            
            // Retrieve and delete the entity in a single operation
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                _logger.LogWarning($"{typeof(T).Name} with ID {id} not found for deletion");
                return false;
            }

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Deleted {typeof(T).Name} with ID {id}");
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting {typeof(T).Name} with ID {id}");
            throw;
        }
    }
}
