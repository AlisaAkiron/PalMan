using PalMan.Shared.Models.PalWorld;

namespace PalMan.Agent.Entities;

public class PalWorldSettings : ServerArguments
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public PalWorldSettings()
    {
    }

    public PalWorldSettings(ServerArguments arguments) : base(arguments)
    {
    }
}
