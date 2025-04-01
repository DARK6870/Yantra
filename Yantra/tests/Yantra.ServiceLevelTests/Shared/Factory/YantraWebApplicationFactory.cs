using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Testcontainers.MongoDb;
using Yantra.ServiceLevelTests.Shared.Helpers;

namespace Yantra.ServiceLevelTests.Shared.Factory;

public class YantraWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer =
        new MongoDbBuilder()
            .WithUsername("admin")
            .WithPassword("admin")
            .Build();

    private string MongoDbConnectionString => _mongoDbContainer.GetConnectionString();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("MongoDb__ConnectionString", MongoDbConnectionString);
        Environment.SetEnvironmentVariable("AuthenticationOptions__EnableSecurity", bool.TrueString);
        
        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        // Start Containers
        await _mongoDbContainer.StartAsync();
        
        // Add Some Test Data
        await Services.MigrateTestData();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}