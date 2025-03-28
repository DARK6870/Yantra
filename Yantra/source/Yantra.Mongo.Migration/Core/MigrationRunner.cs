using System.Reflection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Mongo.Migration.Core;

public class MigrationRunner(
    IMongoDatabase database,
    IMigrationsRepository migrationsRepository,
    ILogger<MigrationRunner> logger
)
{
    public async Task ExecuteMigrationsAsync(Assembly assembly)
    {
        var migrations = GetMigrations(assembly);

        foreach (var migration in migrations)
        {
            if (Activator.CreateInstance(migration, database) is Migration migrationInstance
                && !await migrationsRepository.ExistsAsync(x => x.Description == migrationInstance.Description))
            {
                logger.LogInformation("Running migration '{migrationName}'", migration.Name);

                await migrationInstance.MigrateAsync();
                await migrationsRepository.InsertOneAsync(new MigrationEntity(migrationInstance.Description));
                
                logger.LogInformation("Migration '{migrationName}' executed successfully", migration.Name);
            }
            else
            {
                logger.LogInformation("Migration {migrationName} has already been migrated.", migration.Name);
            }
        }
    }

    private static Type[] GetMigrations(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Migration)) && !t.IsAbstract)
            .ToArray();
    }
}