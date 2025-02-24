namespace Yantra.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MongoCollectionAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}