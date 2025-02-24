using HotChocolate.Execution.Configuration;
using Yantra.GraphQl.Mutation;
using Yantra.GraphQl.Query;
using Yantra.GraphQl.Types;

namespace Yantra;

public static class Configuration
{
    public static IRequestExecutorBuilder AddGraphQlQuery(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        requestExecutorBuilder
            .AddQueryType<GraphQlQuery>()
            .AddTypeExtension<MenuItemQueryType>()
            ;

        return requestExecutorBuilder;
    }
    
    public static IRequestExecutorBuilder AddGraphQlMutation(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        requestExecutorBuilder
            .AddMutationType<GraphQlMutation>()
            .AddTypeExtension<MenuItemMutationType>()
            ;

        return requestExecutorBuilder;
    }
}