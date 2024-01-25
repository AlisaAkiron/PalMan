using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Constants;
using PalMan.Agent.Entities;

namespace PalMan.Agent.Database;

public class PalManDbContext : DbContext
{
    public DbSet<ContainerEvent> ContainerEvents { get; set; } = null!;
    public DbSet<PalWorldServer> PalWorldServers { get; set; } = null!;
    public DbSet<PalWorldSettings> PalWorldSettings { get; set; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (Directory.Exists(AgentConfiguration.DataDirectory) is false)
        {
            Directory.CreateDirectory(AgentConfiguration.DataDirectory);
        }

        var databaseFile = Path.Combine(AgentConfiguration.DataDirectory, "palman-agent.db");
        optionsBuilder.UseSqlite($"Data Source={databaseFile};");
    }
}
