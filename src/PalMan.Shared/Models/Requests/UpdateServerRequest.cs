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
}
