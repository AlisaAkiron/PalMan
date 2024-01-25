using System.Text.Json.Serialization;
using PalMan.Shared.Abstract;
using PalMan.Shared.Attributes;
using PalMan.Shared.Enums;
using PalMan.Shared.Models.PalWorld;

namespace PalMan.Shared.Models.Requests;

[AgentEndpoint("server/update", AgentRequestMethod.POST)]
public class UpdateServerRequest : IAgentRequestData
{
    [JsonPropertyName("identifier")]
    public string Identifier { get; set; } = null!;

    [JsonPropertyName("server_arguments")]
    public ServerArguments ServerArguments { get; set; } = new();

    [JsonPropertyName("change_list")]
    public List<string> ChangeList { get; set; } = [];

    [JsonPropertyName("shutdown_delay")]
    public int ShutdownDelay { get; set; } = 30;

    [JsonPropertyName("shutdown_delay_message")]
    public string ShutdownDelayMessage { get; set; } = string.Empty;

    [JsonPropertyName("force_shutdown")]
    public bool ForceShutdown { get; set; }
}
