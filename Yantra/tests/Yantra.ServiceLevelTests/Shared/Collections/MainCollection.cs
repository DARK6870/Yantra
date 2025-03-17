using Yantra.ServiceLevelTests.Shared.Factory;

namespace Yantra.ServiceLevelTests.Shared.Collections;

[CollectionDefinition(nameof(MainCollection))]
public class MainCollection : ICollectionFixture<YantraWebApplicationFactory>
{
    
}