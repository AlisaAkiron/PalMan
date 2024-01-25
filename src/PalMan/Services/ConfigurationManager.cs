using System.Text.Json;
using PalMan.Configuration;
using PalMan.Interfaces;

namespace PalMan.Services;

public class ConfigurationManager : IConfigurationManager
{
    private readonly FileInfo _palManAgentFile;

    public ConfigurationManager()
    {
        string configBaseDirectory;

        // IF PALMAN_CONFIG_ROOT IS SET, USE THAT
        if (Environment.GetEnvironmentVariable("PALMAN_CONFIG_ROOT") is { } configRoot)
        {
            configBaseDirectory = configRoot;
        }

        // IF LINUX, LOAD FROM ~/.config/alisa-lab/palman
        else if (OperatingSystem.IsLinux())
        {
            // IF XDG_USER_CONFIG_DIR IS SET, USE THAT
            // ELSE USE ~/.config/alisa-lab/palman
            if (Environment.GetEnvironmentVariable("XDG_USER_CONFIG_DIR") is { } xdgUserConfigDir)
            {
                configBaseDirectory = Path.Combine(xdgUserConfigDir, "alisa-lab", "palman");
            }
            else
            {
                configBaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "alisa-lab", "palman");
            }
        }

        // IF WINDOWS, LOAD FROM %APPDATA%/alisa-lab/palman
        else if (OperatingSystem.IsWindows())
        {
            configBaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "alisa-lab", "palman");
        }

        // IF MACOS, LOAD FROM ~/.config/alisa-lab/palman
        else if (OperatingSystem.IsMacOS())
        {
            configBaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "alisa-lab", "palman");
        }

        // IF UNKNOWN, THROW EXCEPTION
        else
        {
            throw new PlatformNotSupportedException("Unsupported platform");
        }

        // GET FULL PATH
        configBaseDirectory = Path.GetFullPath(configBaseDirectory);

        // ENSURE CONFIG DIRECTORY EXISTS
        Directory.CreateDirectory(configBaseDirectory);

        // SET FILE INFO
        _palManAgentFile = new FileInfo(Path.Combine(configBaseDirectory, "palman-agent.json"));

        // ENSURE FILES EXIST
        if (!_palManAgentFile.Exists)
        {
            File.WriteAllText(_palManAgentFile.FullName, "[]");
        }
    }

    public async Task<List<PalManAgent>> GetAgents()
    {
        var json = await File.ReadAllTextAsync(_palManAgentFile.FullName);
        var objs = JsonSerializer.Deserialize<List<PalManAgent>>(json)
            ?? throw new JsonException("Failed to deserialize palman-agent.json");
        return objs;
    }

    public async Task<PalManAgent?> GetAgent(string name)
    {
        var agents = await GetAgents();
        return agents.FirstOrDefault(x => x.Name == name);
    }

    public async Task AddAgent(PalManAgent agent)
    {
        var agents = await GetAgents();
        agents.Add(agent);
        var json = JsonSerializer.Serialize(agents);
        await File.WriteAllTextAsync(_palManAgentFile.FullName, json);
    }

    public async Task<bool> RemoveAgent(string name)
    {
        var agents = await GetAgents();
        var count = agents.RemoveAll(x => x.Name == name);

        if (count == 0)
        {
            return false;
        }

        var json = JsonSerializer.Serialize(agents);
        await File.WriteAllTextAsync(_palManAgentFile.FullName, json);

        return true;
    }
}
