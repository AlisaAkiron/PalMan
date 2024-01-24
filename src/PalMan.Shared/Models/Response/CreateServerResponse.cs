using PalMan.Shared.Abstract;

namespace PalMan.Shared.Models.Response;

public class CreateServerResponse : IAgentResponseData
{
    public Guid Id { get; set; }

    public string ContainerId { get; set; } = string.Empty;
}
