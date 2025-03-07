using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types.Descriptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Yantra.Infrastructure.Authentication;
using Yantra.Infrastructure.Common.Constants;

namespace Yantra.Infrastructure.GraphQl;

public static class GraphQlSetup
{
    public static IRequestExecutorBuilder ConfigureGraphQl(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        if (AuthenticationSetup.EnableSecurity)
            requestExecutorBuilder.AddAuthorization();
        
        requestExecutorBuilder
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddQueryableOffsetPagingProvider(GraphQlConstants.QueryablePaginationProvider)
            .DisableIntrospection(false)
            .AddErrorFilter<GraphQlErrorFilter>()
            .AddConvention<INamingConventions>(new ApplicationNamingConvention())
            .AddInMemorySubscriptions()
            ;

        return requestExecutorBuilder;
    }

    public static void MapGraphQl(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapNitroApp()
            .WithOptions(new GraphQLToolOptions()
            {
                DisableTelemetry = true,
                GaTrackingId = null
            });
        
        endpointRouteBuilder.MapGraphQL();
    }
}