using System.Reflection;

namespace PalMan.Agent.Extensions;

public static class BuilderExtension
{
    public static IConfigurationBuilder AddPalManConfigurations(this IConfigurationBuilder builder)
    {
        var dockerHost = Environment.GetEnvironmentVariable("PALMAN_DOCKER_HOST")
            ?? "unix:///var/run/docker.sock";
        var dockerUsername = Environment.GetEnvironmentVariable("PALMAN_DOCKER_USERNAME")
            ?? string.Empty;
        var dockerPassword = Environment.GetEnvironmentVariable("PALMAN_DOCKER_PASSWORD")
            ?? string.Empty;
        var dataDirectory = Environment.GetEnvironmentVariable("PALMAN_DATA_DIRECTORY")
            ?? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "data");

        builder.AddInMemoryCollection(new[]
        {
            new KeyValuePair<string, string?>("Docker:Host", dockerHost),
            new KeyValuePair<string, string?>("Docker:Username", dockerUsername),
            new KeyValuePair<string, string?>("Docker:Password", dockerPassword),
            new KeyValuePair<string, string?>("DataDirectory", dataDirectory)
        });

        return builder;
    }
}
