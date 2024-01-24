using PalMan.Configuration;

namespace PalMan.Interfaces;

public interface IConfigurationManager
{
    public Task<List<PalManAgent>> GetAgents();

    public Task<PalManAgent?> GetAgent(string name);

    public Task AddAgent(PalManAgent agent);

    public Task RemoveAgent(string name);
}
