using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PalMan.Agent.Entities;
using PalMan.Shared.Enums;

namespace PalMan.Agent.Database;

public class PalManDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<ContainerEvent> ContainerEvents { get; set; } = null!;
    public DbSet<PalWorldServer> PalWorldServers { get; set; } = null!;
    public DbSet<PalWorldSettings> PalWorldSettings { get; set; } = null!;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ContainerEvent>(e =>
        {
            e.HasKey(p => p.Id);
        });

        modelBuilder.Entity<PalWorldServer>(e =>
        {
            e.HasKey(p => p.Id);
        });

        modelBuilder.Entity<PalWorldSettings>(e =>
        {
            e.HasKey(p => p.Id);
        });
    }
}
