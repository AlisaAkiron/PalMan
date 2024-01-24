using System.Text.Json.Serialization;
using PalMan.Shared.Abstract;
using PalMan.Shared.Attributes;
using PalMan.Shared.Enums;
using PalMan.Shared.Models.PalWorld;

namespace PalMan.Shared.Models.Requests;

[AgentEndpoint("server/create", AgentRequestMethod.POST)]
public class CreateServerRequest : IAgentRequestData
{
    [JsonPropertyName("identifier")]
    public string Identifier { get; set; } = null!;

    [JsonPropertyName("server_arguments")]
    public ServerArguments ServerArguments { get; set; } = new();
}
