using System.Net;
using CoreRCON;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Constants;
using PalMan.Agent.Database;
using PalMan.Agent.Extensions;
using PalMan.Agent.Queue.Abstract;
using PalMan.Agent.Queue.Models;
using PalMan.Shared.Utils;

namespace PalMan.Agent.Queue.Executor;

public class UpdateServerTaskExecutor : ITaskQueueExecutor<UpdateServerTask>
{
    private readonly IDockerClient _docker;
    private readonly PalManDbContext _dbContext;

    public UpdateServerTaskExecutor(IDockerClient docker, PalManDbContext dbContext)
    {
        _docker = docker;
        _dbContext = dbContext;
    }

    public async Task Execute(UpdateServerTask data, CancellationToken token)
    {
        var server = await _dbContext.PalWorldServers
            .Include(x => x.Settings)
            .FirstAsync(x => x.Identifier == data.RawRequest.Identifier, token);

        var info = await _docker.Containers.InspectContainerAsync(server.ContainerId, token);
        var ip = IPAddress.Parse(info.NetworkSettings.IPAddress);

        using var rcon = new RCON(ip, (ushort)server.Settings.RCONPort, server.Settings.AdminPassword);
        await rcon.ConnectAsync();

        int delay;
        if (data.RawRequest.ForceShutdown)
        {
            await rcon.SaveGame();
            await rcon.ForceShutdown();
            delay = 10;
        }
        else
        {
            await rcon.DelayedShutdown(data.RawRequest.ShutdownDelay, data.RawRequest.ShutdownDelayMessage);
            delay = data.RawRequest.ShutdownDelay;
        }

        await _docker.Containers.StopContainerAsync(server.ContainerId, new ContainerStopParameters
        {
            WaitBeforeKillSeconds = (uint)delay
        }, token);

        var serverSettingsIni = ServerArgumentsSerializer.Serialize(data.UpdatedSettings);
        var serverSettingsPath = Path.Combine(
            DockerConfiguration.VolumeRoot,
            server.Identifier,
            "Pal", "Saved", "Config", "LinuxServer", "PalWorldSettings.ini");

        await File.WriteAllTextAsync(serverSettingsPath, serverSettingsIni, token);

        await _docker.Containers.StartContainerAsync(server.ContainerId, new ContainerStartParameters(), token);
    }
}
