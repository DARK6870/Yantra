using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Yantra.Infrastructure.Configurations;

public static class SerilogConfiguration
{
    public static LoggerConfiguration GetLoggerConfiguration()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}"
            )
            .WriteTo.File(
                "Logs/Yantra.log",
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 30 * 1000000, // 30MB
                retainedFileCountLimit: 10,
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information
            )
            .Enrich.FromLogContext();
    }
}