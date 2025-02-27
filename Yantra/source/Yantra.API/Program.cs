using Serilog;
using Yantra;
using Yantra.Application;
using Yantra.GraphQl;
using Yantra.Infrastructure;
using Yantra.Infrastructure.Configurations;
using Yantra.Infrastructure.GraphQl;
using Yantra.Mongo;
using Yantra.Mongo.Models.Enums;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

Log.Logger = SerilogConfiguration
        .GetLoggerConfiguration()
        .CreateLogger();

builder.Host.UseSerilog();

services
    .AddConfiguration(configuration)
    .AddBearerAuthentication(configuration)
    .AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.RequireRole(Role.Admin.ToString()));
        options.AddPolicy("Manager", policy => policy.RequireRole(Role.Manager.ToString(), Role.Admin.ToString()));
        options.AddPolicy("Courier", policy => policy.RequireRole(Role.Courier.ToString(), Role.Admin.ToString(), Role.Manager.ToString()));
    })
    .AddMongoDb(configuration)
    .AddRepositories()
    .AddPipelineBehaviours()
    .AddInfrastructureServices()
    .AddApplicationServices()
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQuery()
    .AddGraphQlMutation()
    ;

var app = builder.Build();

app.MapGraphQl();
app
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<GraphQlStatusCodeMiddleware>()
    ;

app.Run();