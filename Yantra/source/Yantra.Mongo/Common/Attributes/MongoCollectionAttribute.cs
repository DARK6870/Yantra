namespace Yantra.Mongo.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MongoCollectionAttribute(string name) : Attribute
{
    public string Name { get; } = name;
    
    public static string GetCollectionName(Type entityType)
    {
        var attribute = entityType.GetCustomAttributes(typeof(MongoCollectionAttribute), false)
            .FirstOrDefault() as MongoCollectionAttribute;

        return attribute?.Name ?? throw new NullReferenceException("Mongo collection name can not be null");
    }
}