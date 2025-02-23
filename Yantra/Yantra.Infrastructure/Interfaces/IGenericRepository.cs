namespace Yantra.Infrastructure.Interfaces;

public interface IGenericRepository<T> where T : IEntity
{
    IQueryable<T> AsQueryable();
    
    Task<T?> FindByIdAsync(string id);
    
    Task InsertOneAsync(T entity);
    
    Task InsertManyAsync(IEnumerable<T> entities);
    
    Task DeleteByIdAsync(string id);
    
    Task ReplaceOneAsync(T entity);
}