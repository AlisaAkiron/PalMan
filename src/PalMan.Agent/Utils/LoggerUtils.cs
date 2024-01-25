using PalMan.Agent.Constants;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace PalMan.Agent.Utils;

public static class LoggerUtils
{
    public static Logger CreateLogger()
    {
        if (Directory.Exists(AgentConfiguration.LogDirectory) is false)
        {
            Directory.CreateDirectory(AgentConfiguration.LogDirectory);
        }

        var logFile = Path.Combine(AgentConfiguration.LogDirectory, "palman-.log");

        var minimumLevel = AgentConfiguration.RuntimeMode == "Production"
            ? LogEventLevel.Information
            : LogEventLevel.Debug;

        return new LoggerConfiguration()
            .MinimumLevel.Is(minimumLevel)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.File(logFile, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public static void PrintAgentConfiguration()
    {
        Log.Logger.Information("Use data directory: {DataDirectory}", AgentConfiguration.DataDirectory);
        Log.Logger.Information("Use log directory: {LogDirectory}", AgentConfiguration.LogDirectory);
        Log.Logger.Information("Use runtime mode: {RuntimeMode}", AgentConfiguration.RuntimeMode);
    }
}
