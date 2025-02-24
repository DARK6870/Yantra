using System.Reflection;
using MongoDB.Driver;
using Yantra.Infrastructure.Attributes;
using Yantra.Infrastructure.Interfaces;

namespace Yantra.Infrastructure.Repositories;

public class GenericRepository<T>(IMongoDatabase mongoDatabase)
    : IGenericRepository<T> where T : IEntity
{
    protected IMongoCollection<T> Collection => mongoDatabase.GetCollection<T>(GetCollectionName(typeof(T)));

    public IQueryable<T> AsQueryable()
    {
        return Collection.AsQueryable();
    }

    public async Task<T?> FindByIdAsync(string id)
    {
        var findResult = await Collection
            .FindAsync(e => e.Id == id);
        
        return await findResult.SingleOrDefaultAsync();
    }

    public async Task InsertOneAsync(T entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public async Task InsertManyAsync(IEnumerable<T> entities)
    {
        var documents = entities.ToList();
        if (documents.Count == 0)
            return;
        
        await Collection.InsertManyAsync(documents);
    }

    public async Task DeleteByIdAsync(string id)
    {
        await Collection.DeleteOneAsync(e => e.Id == id);
    }

    public async Task ReplaceOneAsync(T entity)
    {
        await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
    }

    private static string GetCollectionName(Type entityType)
    {
        var collectionName = entityType.GetCustomAttribute<MongoCollectionAttribute>()?.Name;
        
        return !string.IsNullOrEmpty(collectionName)
            ? collectionName
            : entityType.Name;
    }
}