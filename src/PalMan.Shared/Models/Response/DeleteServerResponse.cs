using PalMan.Shared.Abstract;

namespace PalMan.Shared.Models.Response;

public class DeleteServerResponse : IAgentResponseData
{
    public Guid TaskId { get; set; }
}
