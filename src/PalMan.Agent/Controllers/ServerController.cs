using Docker.DotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Database;
using PalMan.Agent.Entities;
using PalMan.Agent.Extensions;
using PalMan.Shared.Extensions;
using PalMan.Shared.Models;
using PalMan.Shared.Models.Requests;
using PalMan.Shared.Models.Response;
using PalMan.Shared.Utils;

namespace PalMan.Agent.Controllers;

[ApiController]
[Route("/v1/server")]
public class ServerController : ControllerBase
{
    private readonly IDockerClient _docker;
    private readonly PalManDbContext _dbContext;

    public ServerController(IDockerClient docker, PalManDbContext dbContext)
    {
        _docker = docker;
        _dbContext = dbContext;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateServer(AgentRequest<CreateServerRequest> request)
    {
        var currentServers = await _dbContext.PalWorldServers
            .Include(x => x.Settings)
            .ToListAsync();

        var isNameExist = currentServers.Exists(x => x.Identifier == request.Data.Identifier);
        var isPortExist = currentServers.Exists(x => x.Settings.PublicPort == request.Data.ServerArguments.PublicPort);
        var isRconPortExist = currentServers.Exists(x => x.Settings.RCONPort == request.Data.ServerArguments.RCONPort);

        if (isNameExist || isPortExist || isRconPortExist)
        {
            return BadRequest(new CreateServerResponse().ToResponse("Server name or port or rcon port already exist"));
        }

        var serverArguments = request.Data.ServerArguments;

        // FORCE RCON
        serverArguments.RCONEnabled = true;

        // FORCE ADMIN PASSWORD
        if (string.IsNullOrEmpty(serverArguments.AdminPassword))
        {
            serverArguments.AdminPassword = RandomUtils.GeneratePassword();
        }

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
            .Include(x => x.Settings)
            .FirstOrDefaultAsync(x => x.Identifier == request.Data.Identifier);

        if (server is null)
        {
            return NotFound(new UpdateServerResponse().ToResponse("Server not found"));
        }

        if (server.Installed is false)
        {
            return BadRequest(new UpdateServerResponse().ToResponse("Server not installed"));
        }

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

        // FORCE ADMIN PASSWORD
        if (string.IsNullOrEmpty(server.Settings.AdminPassword))
        {
            server.Settings.AdminPassword = RandomUtils.GeneratePassword();
        }

        await _docker.RestartServerAsync(server);
        await _dbContext.SaveChangesAsync();

        return Ok(new UpdateServerResponse().ToResponse());
    }
}
