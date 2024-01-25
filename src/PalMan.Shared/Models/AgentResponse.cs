using PalMan.Shared.Abstract;

namespace PalMan.Shared.Models;

public class AgentResponse<T> where T : class, IAgentResponseData, new()
{
    public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }
}
