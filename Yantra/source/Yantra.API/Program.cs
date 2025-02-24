using Serilog;
using Yantra;
using Yantra.Application;
using Yantra.Infrastructure;
using Yantra.Infrastructure.GraphQl;
using Yantra.Infrastructure.Logging;
using Yantra.Mongo;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

Log.Logger = SerilogConfiguration
    .GetLoggerConfiguration()
    .CreateLogger()
    ;

builder.Host.UseSerilog();

services
    .AddConfiguration(configuration)
    .AddMongoDb(configuration)
    .AddRepositories()
    .AddLoggingBehavior()
    .AddServices()
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQuery()
    .AddGraphQlMutation()
    ;

var app = builder.Build();

app.MapGraphQl();
app.UseMiddleware<GraphQlStatusCodeMiddleware>();

app.Run();
