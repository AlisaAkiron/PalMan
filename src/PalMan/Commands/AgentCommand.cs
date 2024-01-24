using Cocona;
using PalMan.Configuration;
using PalMan.Interfaces;
using Spectre.Console;

namespace PalMan.Commands;

public class AgentCommand
{
    private readonly IConfigurationManager _configurationManager;
    private readonly IAgentClient _client;

    public AgentCommand(IConfigurationManager configurationManager, IAgentClient client)
    {
        _configurationManager = configurationManager;
        _client = client;
    }

    [Command("add", Description = "Add a new Agent")]
    public async Task AddAgent(
        [Option('u', Description = "Agent server URL")] string url,
        [Option('t', Description = "Agent server enrollment token")] string token,
        [Option('n', Description = "Agent server alias name")] string name)
    {
        var agents = await _configurationManager.GetAgents();
        if (agents.Exists(x => x.Name == name))
        {
            AnsiConsole.WriteLine("[red]The agent with the same name already exist.[/]");
        }

        var agent = new PalManAgent
        {
            Name = name,
            Token = token,
            Url = url
        };

        var pong = await _client.PingAsync(agent);
        if (pong is false)
        {
            AnsiConsole.WriteLine("[red]The agent is not responding.[/]");
        }

        await _configurationManager.AddAgent(agent);
        AnsiConsole.WriteLine("[green]The agent has been added successfully.[/]");
    }
}
