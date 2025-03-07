using HotChocolate.Execution.Configuration;
using Yantra.GraphQl.Subscription;
using Yantra.GraphQl.Types;
using Yantra.GraphQl.Types.MutationTypes;
using Yantra.GraphQl.Types.QueryTypes;

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
            .AddTypeExtension<OrderQueryType>()
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
    
    public static IRequestExecutorBuilder AddGraphQlSubscription(
        this IRequestExecutorBuilder requestExecutorBuilder
    )
    {
        requestExecutorBuilder
            .AddSubscriptionType<Subscription.Subscription>()
            .AddTypeExtension<OrderSubscription>()
            ;

        return requestExecutorBuilder;
    }
}