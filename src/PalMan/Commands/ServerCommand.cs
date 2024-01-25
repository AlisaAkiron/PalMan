using Cocona;
using PalMan.Interfaces;
using PalMan.Shared.Extensions;
using PalMan.Shared.Models.PalWorld;
using PalMan.Shared.Models.Requests;
using PalMan.Shared.Models.Response;
using PalMan.Utils;
using Spectre.Console;

namespace PalMan.Commands;

public class ServerCommand
{
    private readonly IConfigurationManager _configurationManager;
    private readonly IAgentClient _client;

    public ServerCommand(IConfigurationManager configurationManager, IAgentClient client)
    {
        _configurationManager = configurationManager;
        _client = client;
    }

    [Command("create", Description = "Create a new Palworld server")]
    public async Task CreateAsync(
        [Option('a', Description = "Agent name")] string agent,
        [Option('i', Description = "Server name")] string identifier,
        [Option(Description = "(PalWorldSettings.ini) Server port")] int? port = null,
        [Option(Description = "(PalWorldSettings.ini) Server port")] int? rconPort = null,
        [Option(Description = "(PalWorldSettings.ini) Server name")] string? name = null,
        [Option(Description = "(PalWorldSettings.ini) Server description")] string? description = null,
        [Option(Description = "(PalWorldSettings.ini) Server password")] string? password = null,
        [Option(Description = "(PalWorldSettings.ini) Server admin Password, if empty, a new one will be generated")] string? adminPassword = null,
        [Option(Description = "(PalWorldSettings.ini) Server max players")] int? maxPlayers = null,
        [Option(Description = "(PalWorldSettings.ini) Server max guild players")] int? maxGuildPlayers = null,
        [Option(Description = "(PalWorldSettings.ini) PvP")] bool? pvp = null,
        [Option(Description = "If false, arguments from --set will be overwritten by options like --port")] bool forceArguments = false,
        [Option('s', Description = "Set server arguments")] IEnumerable<string>? set = null)
    {
        var palManAgent = await _configurationManager.GetAgent(agent);
        if (palManAgent is null)
        {
            AnsiConsole.WriteLine("[red]Agent not found[/]");
            return;
        }

        var serverArguments = set is null ? new ServerArguments() : ServerArgumentUtils.Parse(set);

        if (serverArguments is null)
        {
            AnsiConsole.WriteLine("[red]Failed to parse server arguments[/]");
            return;
        }

        if (forceArguments is false)
        {
            serverArguments.PublicPort = port ?? serverArguments.PublicPort;
            serverArguments.RCONPort = rconPort ?? serverArguments.RCONPort;
            serverArguments.ServerName = name ?? serverArguments.ServerName;
            serverArguments.ServerDescription = description ?? serverArguments.ServerDescription;
            serverArguments.ServerPassword = password ?? serverArguments.ServerPassword;
            serverArguments.AdminPassword = adminPassword ?? serverArguments.AdminPassword;
            serverArguments.ServerPlayerMaxNum = maxPlayers ?? serverArguments.ServerPlayerMaxNum;
            serverArguments.GuildPlayerMaxNum = maxGuildPlayers ?? serverArguments.GuildPlayerMaxNum;
            serverArguments.bIsPvP = pvp ?? serverArguments.bIsPvP;
        }

        var request = new CreateServerRequest
        {
            Identifier = identifier,
            ServerArguments = serverArguments,
        };

        var response = await _client.RequestAsync<CreateServerRequest, CreateServerResponse>
            (palManAgent, request.ToRequest());

        AnsiConsole.WriteLine("[green]Server created[/]");
        AnsiConsole.WriteLine($"Server ID: [yellow]{response.Id}[/]");
        AnsiConsole.WriteLine($"Container ID: [yellow]{response.ContainerId}[/]");
    }

    [Command("update", Description = "Update Palworld server settings")]
    public async Task UpdateAsync(
        [Option('a', Description = "Agent name")] string agent,
        [Option('i', Description = "Server name")] string identifier,
        [Option(Description = "(PalWorldSettings.ini) Server port")] int? port = null,
        [Option(Description = "(PalWorldSettings.ini) Server port")] int? rconPort = null,
        [Option(Description = "(PalWorldSettings.ini) Server name")] string? name = null,
        [Option(Description = "(PalWorldSettings.ini) Server description")] string? description = null,
        [Option(Description = "(PalWorldSettings.ini) Server password")] string? password = null,
        [Option(Description = "(PalWorldSettings.ini) Server admin Password, if empty, a new one will be generated")] string? adminPassword = null,
        [Option(Description = "(PalWorldSettings.ini) Server max players")] int? maxPlayers = null,
        [Option(Description = "(PalWorldSettings.ini) Server max guild players")] int? maxGuildPlayers = null,
        [Option(Description = "(PalWorldSettings.ini) PvP")] bool? pvp = null,
        [Option(Description = "If false, arguments from --set will be overwritten by options like --port")] bool forceArguments = false,
        [Option('s', Description = "Set server arguments")] IEnumerable<string>? set = null)
    {
        var palManAgent = await _configurationManager.GetAgent(agent);
        if (palManAgent is null)
        {
            AnsiConsole.WriteLine("[red]Agent not found[/]");
            return;
        }

        var serverArguments = set is null ? new ServerArguments() : ServerArgumentUtils.Parse(set);

        if (serverArguments is null)
        {
            AnsiConsole.WriteLine("[red]Failed to parse server arguments[/]");
            return;
        }

        if (forceArguments is false)
        {
            serverArguments.PublicPort = port ?? serverArguments.PublicPort;
            serverArguments.RCONPort = rconPort ?? serverArguments.RCONPort;
            serverArguments.ServerName = name ?? serverArguments.ServerName;
            serverArguments.ServerDescription = description ?? serverArguments.ServerDescription;
            serverArguments.ServerPassword = password ?? serverArguments.ServerPassword;
            serverArguments.AdminPassword = adminPassword ?? serverArguments.AdminPassword;
            serverArguments.ServerPlayerMaxNum = maxPlayers ?? serverArguments.ServerPlayerMaxNum;
            serverArguments.GuildPlayerMaxNum = maxGuildPlayers ?? serverArguments.GuildPlayerMaxNum;
            serverArguments.bIsPvP = pvp ?? serverArguments.bIsPvP;
        }

        var changeList = ServerArgumentUtils.GetChangeList(serverArguments).ToList();

        if (changeList.Count == 0)
        {
            AnsiConsole.WriteLine("[red]No changes to apply[/]");
            return;
        }

        var table = new Table();
        table.AddColumns("Property", "New Value");

        foreach (var propertyName in changeList)
        {
            var value = typeof(ServerArguments).GetProperty(propertyName)!.GetValue(serverArguments)!;
            table.AddRow(propertyName, value.ToString()!);
        }

        var confirm = AnsiConsole.Confirm("Confirm changes?", false);

        if (confirm is false)
        {
            AnsiConsole.WriteLine("[red]Aborted[/]");
            return;
        }

        var request = new UpdateServerRequest
        {
            Identifier = identifier,
            ServerArguments = serverArguments,
            ChangeList = changeList
        };

        await _client.RequestAsync<UpdateServerRequest, UpdateServerResponse>
            (palManAgent, request.ToRequest());

        AnsiConsole.WriteLine("[green]Server updated[/]");
    }
}
