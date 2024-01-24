using System.Reflection;

namespace PalMan.Agent.Constants;

public static class AgentConfiguration
{
    public static string DockerImage => Environment.GetEnvironmentVariable("PALMAN_DOCKER_IMAGE")
                                        ?? "ghcr.io/liamsho/palworld-server:latest";
    public static string VolumeRoot => Environment.GetEnvironmentVariable("PALMAN_VOLUME_ROOT")
                                       ?? "/var/lib/palworld-server";
    public static string DockerHost => Environment.GetEnvironmentVariable("PALMAN_DOCKER_HOST")
                                       ?? "unix:///var/run/docker.sock";
    public static string DockerUsername => Environment.GetEnvironmentVariable("PALMAN_DOCKER_USERNAME")
                                          ?? string.Empty;
    public static string DockerPassword => Environment.GetEnvironmentVariable("PALMAN_DOCKER_PASSWORD")
                                          ?? string.Empty;
    public static string DockerNetwork => Environment.GetEnvironmentVariable("PALMAN_DOCKER_NETWORK")
                                         ?? "palman";
    public static string DataDirectory => Environment.GetEnvironmentVariable("PALMAN_DATA_DIRECTORY")
                                         ?? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "data");
}
