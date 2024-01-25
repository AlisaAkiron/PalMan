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
            AnsiConsole.MarkupLine("[red]The agent with the same name already exist.[/]");
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
            AnsiConsole.MarkupLine("[red]The agent is not responding.[/]");
            return;
        }

        await _configurationManager.AddAgent(agent);
        AnsiConsole.MarkupLine("[green]The agent has been added successfully.[/]");
    }

    [Command("rm", Description = "Remove an agent")]
    public async Task RemoveAgent(
        [Option('n', Description = "Agent server alias name")] string name)
    {
        var result = await _configurationManager.RemoveAgent(name);

        if (result is false)
        {
            AnsiConsole.MarkupLine("[red]The agent does not exist.[/]");
            return;
        }

        AnsiConsole.MarkupLine("[green]The agent has been removed successfully.[/]");
    }

    [Command("ls", Description = "List all agents")]
    public async Task ListAgents(
        [Option('t', Description = "Include token in the output")] bool token = false,
        [Option('c', Description = "Include agent health check")] bool check = false)
    {
        var agents = await _configurationManager.GetAgents();

        var table = new Table();

        AnsiConsole.Live(table)
            .Start(ctx =>
            {
                table.AddColumns("Name", "URL");

                if (token)
                {
                    table.AddColumn("Token");
                }

                ctx.Refresh();

                if (check)
                {
                    table.AddColumn("Health");
                }
                ctx.Refresh();

                foreach (var agent in agents)
                {
                    var row = new List<string>
                    {
                        agent.Name, agent.Url
                    };

                    if (token)
                    {
                        row.Add(agent.Token);
                    }

                    if (check)
                    {
                        row.Add("[gray]Waiting...[/]");
                    }

                    table.AddRow(row.ToArray());
                    ctx.Refresh();
                }

                if (check is false)
                {
                    return;
                }

                agents.AsParallel().ForAll(x =>
                {
                    var pong = _client.PingAsync(x).ConfigureAwait(false).GetAwaiter().GetResult();

                    var index = agents.IndexOf(x);
                    table.Rows.Update(index, 3, new Markup(pong ? "[green]HEALTHY[/]" : "[red]NO RESPONSE[/]"));

                    ctx.Refresh();
                });
            });
    }
}
