using PalMan.Shared.Enums;
using PalMan.Shared.Models.PalWorld;

namespace PalMan.Agent.Entities;

public class PalWorldServer
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ContainerId { get; set; } = string.Empty;

    public string Identifier { get; set; } = null!;

    public bool Installed { get; set; }

    public PalWorldSettings Settings { get; set; } = new();
}
