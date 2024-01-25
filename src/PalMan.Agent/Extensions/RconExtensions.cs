using System.Diagnostics.CodeAnalysis;
using CoreRCON;
using PalMan.Shared.Models.Common;

namespace PalMan.Agent.Extensions;

[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
public static class RconExtensions
{
    public static async Task<string?> GetPalworldVersion(this RCON rcon)
    {
        const string command = "Info";

        var info = await rcon.SendCommandAsync(command);

        // Welcome to Pal Server[v0.1.2.0] Wonder Land
        // Welcome to Pal Server[vx.y.z.p] {Server Name}

        try
        {
            var version = info
                .Split('[')[1]
                .Split(']')[0];

            return version;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task<(bool, string?)> SaveGame(this RCON rcon)
    {
        const string command = "Save";
        const string success = "Save complete";

        var result = await rcon.SendCommandAsync(command);

        return (result == success, result);
    }

    public static async Task<string?> DelayedShutdown(this RCON rcon, int seconds, string message = "")
    {
        const string command = "Shutdown {0} {1}";

        message = string.IsNullOrEmpty(message)
            ? $"Server will be shutdown after {seconds} seconds."
            : message;

        var parsedCommand = string.Format(command, seconds, message);

        var result = await rcon.SendCommandAsync(parsedCommand);

        return result;
    }

    public static async Task<string?> ForceShutdown(this RCON rcon)
    {
        const string command = "DoExit";

        var result = await rcon.SendCommandAsync(command);

        return result;
    }

    public static async Task<string?> Broadcast(this RCON rcon, string message)
    {
        const string command = "Broadcast {0}";

        // Space is not supported
        message = message.Replace(' ', '_');

        var parsedCommand = string.Format(command, message);

        var result = await rcon.SendCommandAsync(parsedCommand);

        return result;
    }

    public static async Task<List<PalworldPlayer>?> GetPlayers(this RCON rcon)
    {
        const string command = "ShowPlayers";

        var response = await rcon.SendCommandAsync(command);

        var players = response?.Split('\n')    // Linux server only
            .Skip(1)
            .Select(x => x.Split(','))
            .Select(x => new PalworldPlayer
            {
                Name = x[0],
                PlayerUid = x[1],
                SteamId = x[2]
            })
            .ToList();

        return players;
    }
}
