using AppAny.HotChocolate.FluentValidation;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Yantra.Infrastructure.GraphQl;

public static class GraphQlSetup
{
    public static IRequestExecutorBuilder ConfigureGraphQl(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        requestExecutorBuilder
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .DisableIntrospection(false)
            .AddErrorFilter<GraphQlErrorFilter>()
            .AddFluentValidation()
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