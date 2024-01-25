using Cocona;

namespace PalMan.Commands;

[HasSubCommands(typeof(AgentCommand), "agent", Description = "Manage PalMan agents")]
[HasSubCommands(typeof(ServerCommand), "server", Description = "Manage PalWorld servers")]
[HasSubCommands(typeof(InfoCommand), "info", Description = "Get information")]
public class RootCommands;
