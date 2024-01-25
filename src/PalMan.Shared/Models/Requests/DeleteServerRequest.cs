using System.Text.Json.Serialization;
using PalMan.Shared.Abstract;

namespace PalMan.Shared.Models.Requests;

public class DeleteServerRequest : IAgentRequestData
{
    [JsonPropertyName("identifier")]
    public string Identifier { get; set; } = string.Empty;

    [JsonPropertyName("remove_data")]
    public bool RemoveData { get; set; }
}
