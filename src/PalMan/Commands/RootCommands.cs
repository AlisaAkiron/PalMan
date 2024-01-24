using Cocona;

namespace PalMan.Commands;

[HasSubCommands(typeof(AgentCommand), "agent", Description = "Manage PalMan agents")]
public class RootCommands;
