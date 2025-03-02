using HotChocolate.Execution.Configuration;
using Yantra.GraphQl.Types;

namespace Yantra.GraphQl;

public static class Configuration
{
    public static IRequestExecutorBuilder AddGraphQlQuery(
        this IRequestExecutorBuilder requestExecutorBuilder
    )
    {
        requestExecutorBuilder
            .AddQueryType<Query.Query>()
            .AddTypeExtension<MenuItemQueryType>()
            .AddTypeExtension<UserQueryType>()
            .AddTypeExtension<AuthenticationQueryType>()
            ;

        return requestExecutorBuilder;
    }

    public static IRequestExecutorBuilder AddGraphQlMutation(
        this IRequestExecutorBuilder requestExecutorBuilder
    )
    {
        requestExecutorBuilder
            .AddMutationType<Mutation.Mutation>()
            .AddTypeExtension<AuthenticationMutationType>()
            .AddTypeExtension<MenuItemMutationType>()
            .AddTypeExtension<UserMutationType>()
            .AddTypeExtension<OrderMutationType>()
            ;

        return requestExecutorBuilder;
    }
}