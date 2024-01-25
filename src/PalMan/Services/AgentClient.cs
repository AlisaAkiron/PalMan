using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using PalMan.Configuration;
using PalMan.Interfaces;
using PalMan.Shared.Abstract;
using PalMan.Shared.Attributes;
using PalMan.Shared.Extensions;
using PalMan.Shared.Models;
using Spectre.Console;

namespace PalMan.Services;

[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
public class AgentClient : IAgentClient, IDisposable
{
    private readonly HttpClient _client = new();

    public async Task<bool> PingAsync(PalManAgent agent)
    {
        try
        {
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{agent.Url}/ping"),
                Headers =
                {
                    { "Authorization", $"Bearer {agent.Token}" }
                }
            };

            var response = await _client.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode is false)
            {
                return false;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString == "pong";
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return false;
        }
    }

    public async Task<TResponse> RequestAsync<TRequest, TResponse>(PalManAgent agent, AgentRequest<TRequest> request)
        where TResponse : class, IAgentResponseData, new()
        where TRequest : IAgentRequestData, new()
    {
        try
        {
            var attribute = typeof(TRequest).GetCustomAttribute<AgentEndpointAttribute>()!;
            var httpRequest = new HttpRequestMessage
            {
                Method = attribute.Method.ToMethod(),
                RequestUri = new Uri($"{agent.Url}{attribute.Endpoint}"),
                Content = new StringContent(request.ToRequestString()),
                Headers =
                {
                    { "Authorization", $"Bearer {agent.Token}" },
                    { "Content-Type", "application/json" }
                }
            };

            var response = await _client.SendAsync(httpRequest);

            var body = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<AgentResponse<TResponse>>(body)!;

            if (response.IsSuccessStatusCode is false || responseData.Data is null)
            {
                throw new HttpRequestException($"Request failed, {responseData.Message}");
            }

            return responseData.Data!;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            throw;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _client.Dispose();

        GC.ReRegisterForFinalize(this);
    }
}
