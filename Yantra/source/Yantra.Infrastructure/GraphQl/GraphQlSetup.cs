﻿using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types.Descriptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Yantra.Infrastructure.GraphQl;

public static class GraphQlSetup
{
    public static IRequestExecutorBuilder ConfigureGraphQl(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        requestExecutorBuilder
            .AddAuthorization()
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .DisableIntrospection(false)
            .AddErrorFilter<GraphQlErrorFilter>()
            .AddConvention<INamingConventions>(new ApplicationNamingConvention())
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