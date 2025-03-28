using System.Linq.Expressions;
using Yantra.Mongo.Models.Entities.Generic;

namespace Yantra.Mongo.Repositories.Interfaces;

public interface IGenericRepository<T> where T : IEntity
{
    IQueryable<T> AsQueryable();
    
    Task<T?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    
    Task InsertOneAsync(T entity, CancellationToken cancellationToken = default);
    
    Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    
    Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default);
    
    Task ReplaceOneAsync(T entity, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
}