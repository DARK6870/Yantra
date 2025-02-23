using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Yantra.Infrastructure.Logging;

public static class SerilogConfiguration
{
    public static LoggerConfiguration GetLoggerConfiguration()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
            )
            .WriteTo.File(
                "Logs/Yantra.log",
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 30 * 1000000, // 30MB
                retainedFileCountLimit: 10,
                rollingInterval: RollingInterval.Day
            )
            .Enrich.FromLogContext();
        
    }
}