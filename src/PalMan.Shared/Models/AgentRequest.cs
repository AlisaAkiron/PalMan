using System.Text.Json.Serialization;
using PalMan.Shared.Abstract;

namespace PalMan.Shared.Models;

public class AgentRequest<T> where T : IAgentRequestData, new()
{
    [JsonPropertyName("time")]
    public DateTimeOffset Time { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; } = new();
}
