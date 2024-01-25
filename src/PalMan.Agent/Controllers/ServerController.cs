using System.Net;
using CoreRCON;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Constants;
using PalMan.Agent.Database;
using PalMan.Agent.Entities;
using PalMan.Agent.Extensions;
using PalMan.Agent.Queue;
using PalMan.Agent.Queue.Models;
using PalMan.Agent.Validators.Common;
using PalMan.Shared.Extensions;
using PalMan.Shared.Models;
using PalMan.Shared.Models.Requests;
using PalMan.Shared.Models.Response;

namespace PalMan.Agent.Controllers;

[ApiController]
[Route("/v1/server")]
public class ServerController : ControllerBase
{
    private readonly IDockerClient _docker;
    private readonly PalManDbContext _dbContext;
    private readonly UpdateServerTaskQueue _updateServerTaskQueue;

    public ServerController(
        IDockerClient docker,
        PalManDbContext dbContext,
        UpdateServerTaskQueue updateServerTaskQueue)
    {
        _docker = docker;
        _dbContext = dbContext;
        _updateServerTaskQueue = updateServerTaskQueue;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateServer(AgentRequest<CreateServerRequest> request)
    {
        var serverArguments = request.Data.ServerArguments;

        // FORCE RCON
        serverArguments.RCONEnabled = true;

        var server = new PalWorldServer
        {
            Id = Guid.NewGuid(),
            ContainerId = string.Empty,
            Identifier = request.Data.Identifier,
            Installed = false,
            Settings = new PalWorldSettings(serverArguments),
        };

        var containerId = await _docker.StartInstallationAsync(server);

        server.ContainerId = containerId;

        await _dbContext.PalWorldServers.AddAsync(server);
        await _dbContext.SaveChangesAsync();

        return Ok(new CreateServerResponse
        {
            Id = server.Id,
            ContainerId = server.ContainerId
        }.ToResponse());
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateServer(AgentRequest<UpdateServerRequest> request)
    {
        var server = await _dbContext.PalWorldServers
            .AsNoTracking()
            .Include(x => x.Settings)
            .FirstAsync(x => x.Identifier == request.Data.Identifier);

        // Update server settings
        foreach (var propertyNames in request.Data.ChangeList)
        {
            var property = typeof(PalWorldSettings).GetProperty(propertyNames)!;

            var value = property.GetValue(request.Data.ServerArguments);

            if (value is null)
            {
                continue;
            }

            property.SetValue(server.Settings, value);
        }

        // FORCE RCON
        server.Settings.RCONEnabled = true;

        var existingServerSettings = await _dbContext.PalWorldSettings
            .AsNoTracking()
            .Where(x => x.Id != server.Settings.Id)
            .ToListAsync();
        var serverSettingsValidator = new ServerSettingsValidator(existingServerSettings);
        var validationResult = await serverSettingsValidator.ValidateAsync(server.Settings);

        if (validationResult.IsValid is false)
        {
            var message = string.Join(';', validationResult.Errors.Select(x => x.ErrorMessage));
            return BadRequest(message.ToFailedResponse());
        }

        var task = new UpdateServerTask
        {
            RawRequest = request.Data,
            UpdatedSettings = server.Settings
        };

        _updateServerTaskQueue.Add(task);

        return Ok(new UpdateServerResponse
        {
            TaskId = task.TaskId
        }.ToResponse());
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteServer(AgentRequest<DeleteServerRequest> request)
    {
        var server = await _dbContext.PalWorldServers
            .Include(x => x.Settings)
            .FirstAsync(x => x.Identifier == request.Data.Identifier);

        var info = await _docker.Containers.InspectContainerAsync(server.ContainerId);
        var ip = IPAddress.Parse(info.NetworkSettings.IPAddress);

        using var rcon = new RCON(ip, (ushort)server.Settings.RCONPort, server.Settings.AdminPassword);
        await rcon.ConnectAsync();
        await rcon.ForceShutdown();

        await _docker.Containers.StopContainerAsync(server.ContainerId, new ContainerStopParameters());
        await _docker.Containers.RemoveContainerAsync(server.ContainerId, new ContainerRemoveParameters
        {
            Force = true
        });

        if (request.Data.RemoveData)
        {
            Directory.Delete(Path.Combine(DockerConfiguration.VolumeRoot, server.Identifier), true);
        }

        _dbContext.PalWorldServers.Remove(server);
        await _dbContext.SaveChangesAsync();

        return Ok(new DeleteServerResponse().ToResponse());
    }
}
