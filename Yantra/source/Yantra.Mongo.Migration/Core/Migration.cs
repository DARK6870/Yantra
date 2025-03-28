namespace Yantra.Mongo.Migration.Core;

public abstract class Migration(string description)
{
    public string Description { get; } = description;
    
    public abstract Task MigrateAsync();
}