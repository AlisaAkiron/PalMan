using System.Reflection;

namespace PalMan.Agent.Constants;

public static class AgentConfiguration
{
    public static string DataDirectory => Environment.GetEnvironmentVariable("PALMAN_DATA_DIRECTORY")
                                          ?? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "data");

    public static string RuntimeMode => Environment.GetEnvironmentVariable("PALMAN_RUNTIME_MODE")
                                        ?? "Production";

    public static string LogDirectory => Environment.GetEnvironmentVariable("PALMAN_LOG_DIRECTORY")
                                         ?? Path.Combine(DataDirectory, "logs");
}
