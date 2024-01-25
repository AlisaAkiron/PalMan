using Docker.DotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Database;
using PalMan.Agent.Entities;
using PalMan.Agent.Extensions;
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

    public ServerController(IDockerClient docker, PalManDbContext dbContext)
    {
        _docker = docker;
        _dbContext = dbContext;
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

        await _docker.RestartServerAsync(server);
        await _dbContext.SaveChangesAsync();

        return Ok(new UpdateServerResponse().ToResponse());
    }
}
