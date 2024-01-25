using Docker.DotNet;
using Docker.DotNet.Models;
using PalMan.Agent.Constants;
using PalMan.Agent.Entities;
using PalMan.Shared.Utils;

namespace PalMan.Agent.Extensions;

public static class DockerExtensions
{
    public static async Task<string> StartInstallationAsync(this IDockerClient docker, PalWorldServer server)
    {
        await docker.Containers.RemoveContainerAsync(server.ContainerId, new ContainerRemoveParameters());

        var volume = Path.Combine(DockerConfiguration.VolumeRoot, server.Identifier);

        var containerCreationParameter = new CreateContainerParameters
        {
            Image = DockerConfiguration.DockerImage,
            Env = new List<string>
            {
                "NO_UPDATE=false",
                "NO_EXECUTE=true"
            },
            Tty = true,
            HostConfig = new HostConfig
            {
                Binds =
                {
                    $"{volume}:/palworld"
                },
                RestartPolicy = new RestartPolicy
                {
                    Name = RestartPolicyKind.No
                }
            }
        };

        var container = await docker.Containers.CreateContainerAsync(containerCreationParameter);
        await docker.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());

        return container.ID;
    }

    public static async Task<string> StartNewServerAsync(this IDockerClient docker, PalWorldServer server)
    {
        await docker.Containers.RemoveContainerAsync(server.ContainerId, new ContainerRemoveParameters());

        var volume = Path.Combine(DockerConfiguration.VolumeRoot, server.Identifier);
        var settingsFile = Path.Combine(volume, "Pal", "Saved", "Config", "LinuxServer", "PalWorldSettings.ini");
        var settings = ServerArgumentsSerializer.Serialize(server.Settings);
        await File.WriteAllTextAsync(settingsFile, settings);

        var containerCreationParameter = new CreateContainerParameters
        {
            Image = DockerConfiguration.DockerImage,
            Env = new List<string>
            {
                "NO_UPDATE=false",
                "NO_EXECUTE=false"
            },
            Tty = true,
            HostConfig = new HostConfig
            {
                Binds =
                {
                    $"{volume}:/palworld"
                },
                PortBindings =
                {
                    { $"{server.Settings.PublicPort}/udp", [new PortBinding { HostPort = server.Settings.PublicPort.ToString() }] }
                },
                RestartPolicy = new RestartPolicy
                {
                    Name = RestartPolicyKind.Always,
                    MaximumRetryCount = 10
                }
            }
        };

        var container = await docker.Containers.CreateContainerAsync(containerCreationParameter);
        await docker.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());

        return container.ID;
    }

    public static async Task<bool> StartServerAsync(this IDockerClient dockerClient, PalWorldServer server)
    {
        var status = await dockerClient.Containers.StartContainerAsync(server.ContainerId, new ContainerStartParameters());
        return status;
    }

    public static async Task RestartServerAsync(this IDockerClient docker, PalWorldServer server)
    {
        await docker.Containers.RestartContainerAsync(server.ContainerId, new ContainerRestartParameters());
    }
}
