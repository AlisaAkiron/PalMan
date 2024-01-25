using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Entities;

namespace PalMan.Agent.Database;

public class PalManDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<ContainerEvent> ContainerEvents { get; set; } = null!;
    public DbSet<PalWorldServer> PalWorldServers { get; set; } = null!;
    public DbSet<PalWorldSettings> PalWorldSettings { get; set; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;

    public PalManDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var dataDirectory = _configuration.GetValue<string>("DataDirectory")!;
        if (Directory.Exists(dataDirectory) is false)
        {
            Directory.CreateDirectory(dataDirectory);
        }

        var databaseFile = Path.Combine(dataDirectory, "palman-agent.db");
        optionsBuilder.UseSqlite($"Data Source={databaseFile};");
    }
}
