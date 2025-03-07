using Serilog;
using Yantra.Application;
using Yantra.GraphQl;
using Yantra.Infrastructure;
using Yantra.Infrastructure.Authentication;
using Yantra.Infrastructure.Configurations;
using Yantra.Infrastructure.GraphQl;
using Yantra.Mongo;
using Yantra.Mongo.Migration;
using Yantra.Notifications;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

Log.Logger = SerilogConfiguration
        .GetLoggerConfiguration()
        .CreateLogger();

builder.Host.UseSerilog();

services
    .AddConfiguration(configuration)
    .AddJwtAuthentication(configuration)
    .AddMongoDb(configuration)
    .AddNotificationService(configuration)
    .AddRepositories()
    .AddMongoMigrations()
    .AddPipelineBehaviours()
    .AddInfrastructureServices()
    .AddApplicationServices()
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQuery()
    .AddGraphQlMutation()
    .AddGraphQlSubscription()
    ;

var app = builder.Build();

app.MapGraphQl();
app
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<GraphQlStatusCodeMiddleware>()
    .UseWebSockets()
    ;

await app.ExecuteMigrations();

app.Run();