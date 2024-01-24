using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Database;
using PalMan.Agent.Entities;
using PalMan.Agent.Extensions;

namespace PalMan.Agent.Jobs;

public class ContainerMonitoringJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ContainerMonitoringJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            var scope = _serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PalManDbContext>();
            var docker = scope.ServiceProvider.GetRequiredService<IDockerClient>();

            var servers = dbContext.PalWorldServers.Include(x => x.Settings).ToList();
            foreach (var server in servers)
            {
                var containerInfo = await docker.Containers.InspectContainerAsync(server.ContainerId, stoppingToken);

                // Check if installation has finished
                if (server.Installed is false && containerInfo.State.Status == "exited" && containerInfo.State.ExitCode == 0)
                {
                    server.Installed = true;
                    await dbContext.ContainerEvents.AddAsync(new ContainerEvent
                    {
                        Message = $"Server {server.Identifier} installation finished"
                    }, stoppingToken);

                    await docker.Containers.RemoveContainerAsync(server.ContainerId, new ContainerRemoveParameters(), stoppingToken);
                    var containerId = await docker.StartNewServerAsync(server);
                    server.ContainerId = containerId;
                }

                // Record status
                switch (containerInfo.State.Status)
                {
                    case "created":
                        break;
                    case "restarting":
                        break;
                    case "running":
                        break;
                    case "removing":
                        break;
                    case "paused":
                        break;
                    case "exited":
                        break;
                    case "dead":
                        await dbContext.ContainerEvents.AddAsync(new ContainerEvent
                        {
                            Message = $"Server {server.Identifier} with container id {containerInfo.ID} has died"
                        }, stoppingToken);
                        break;
                    default:
                        await dbContext.ContainerEvents.AddAsync(new ContainerEvent
                        {
                            Message = $"Unknown container status {containerInfo.State.Status} for server {server.Identifier}"
                        }, stoppingToken);
                        break;
                }
            }

            await scope.DisposeAsync();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
