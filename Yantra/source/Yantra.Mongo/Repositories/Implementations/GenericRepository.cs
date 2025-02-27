using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Yantra.Mongo.Common.Attributes;
using Yantra.Mongo.Models.Entities.Generic;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Mongo.Repositories.Implementations;

public class GenericRepository<T>(
    IMongoDatabase mongoDatabase
) : IGenericRepository<T> where T : IEntity
{
    protected IMongoCollection<T> Collection => mongoDatabase.GetCollection<T>(GetCollectionName(typeof(T)));

    public IQueryable<T> AsQueryable()
    {
        return Collection.AsQueryable();
    }

    public async Task<T?> FindByIdAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var findResult = await Collection
            .FindAsync(e => e.Id == id, cancellationToken: cancellationToken);

        return await findResult.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task InsertOneAsync(
        T entity,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task InsertManyAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    )
    {
        var documents = entities.ToList();
        if (documents.Count == 0)
            return;

        await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);
    }

    public async Task DeleteByIdAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.DeleteOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
    }

    public async Task ReplaceOneAsync(
        T entity,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default
    )
    {
        return await Collection.CountDocumentsAsync(filter, new CountOptions(), cancellationToken) > 0L;
    }

    public async Task<bool> ExistsAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        return await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id, cancellationToken) != null;
    }

    private static string GetCollectionName(Type entityType)
    {
        var collectionName = entityType.GetCustomAttribute<MongoCollectionAttribute>()?.Name;

        return !string.IsNullOrEmpty(collectionName)
            ? collectionName
            : entityType.Name;
    }
}