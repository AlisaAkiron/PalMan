using PalMan.Shared.Enums;

namespace PalMan.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AgentEndpointAttribute : Attribute
{
    public string Endpoint { get; }

    public AgentRequestMethod Method { get; set; }

    public AgentEndpointAttribute(string endpoint, AgentRequestMethod method)
    {
        Endpoint = endpoint;
        Method = method;
    }
}
