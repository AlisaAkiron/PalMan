using PalMan.Shared.Abstract;

namespace PalMan.Shared.Models.Response;

public class UpdateServerResponse : IAgentResponseData
{
    public Guid TaskId { get; set; }
}
