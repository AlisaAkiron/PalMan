using System.Text.Json;
using PalMan.Shared.Abstract;
using PalMan.Shared.Enums;
using PalMan.Shared.Models;

namespace PalMan.Shared.Extensions;

public static class AgentExtensions
{
    public static AgentResponse<T> ToResponse<T>(this T data, string message = "") where T : class, IAgentResponseData, new()
    {
        return new AgentResponse<T>
        {
            Message = message,
            Data = data
        };
    }

    public static AgentResponse<FailedResponsePlaceholder> ToFailedResponse(this string message)
    {
        return new AgentResponse<FailedResponsePlaceholder>
        {
            Message = message,
            Data = null
        };
    }

    public static AgentRequest<T> ToRequest<T>(this T data, string message = "") where T : IAgentRequestData, new()
    {
        return new AgentRequest<T>
        {
            Time = DateTimeOffset.UtcNow,
            Data = data
        };
    }

    public static string ToRequestString<T>(this AgentRequest<T> request) where T : IAgentRequestData, new()
    {
        return JsonSerializer.Serialize(request);
    }

    public static HttpMethod ToMethod(this AgentRequestMethod method)
    {
        return method switch
        {
            AgentRequestMethod.GET => HttpMethod.Get,
            AgentRequestMethod.POST => HttpMethod.Post,
            AgentRequestMethod.DELETE => HttpMethod.Delete,
            _ => HttpMethod.Post
        };
    }
}
