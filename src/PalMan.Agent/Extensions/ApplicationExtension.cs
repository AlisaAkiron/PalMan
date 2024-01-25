using Docker.DotNet;
using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Constants;
using PalMan.Agent.Database;
using PalMan.Agent.Entities;
using PalMan.Shared.Utils;

namespace PalMan.Agent.Extensions;

public static class ApplicationExtension
{
    public static async Task InitializeDatabase(this WebApplication app)
    {
        var asyncScope = app.Services.CreateAsyncScope();
        var dbContext = asyncScope.ServiceProvider.GetRequiredService<PalManDbContext>();
        var logger = asyncScope.ServiceProvider.GetRequiredService<ILogger<PalManDbContext>>();

        var migrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (migrations.Any())
        {
            logger.LogInformation("Applying database migrations...");
            await dbContext.Database.MigrateAsync();
        }

        logger.LogInformation("Database initialized");

        var hasTokens = await dbContext.Tokens.AnyAsync();
        if (hasTokens is false)
        {
            logger.LogWarning("No tokens found in database, creating default token...");

            var token = new Token
            {
                Id = Guid.NewGuid(),
                Name = "Default",
                TokenValue = RandomUtils.GeneratePassword()
            };

            await dbContext.Tokens.AddAsync(token);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Default token value: {TokenValue}", token.TokenValue);
        }

        await dbContext.DisposeAsync();

        await asyncScope.DisposeAsync();
    }

    public static async Task InitializeDocker(this WebApplication app)
    {
        var asyncScope = app.Services.CreateAsyncScope();
        var docker = asyncScope.ServiceProvider.GetRequiredService<IDockerClient>();
        var logger = asyncScope.ServiceProvider.GetRequiredService<ILogger<DockerClient>>();

        logger.LogDebug("Initializing Docker...");
        logger.LogInformation("Use Docker host: {DockerHost}", DockerConfiguration.DockerHost);

        try
        {
            var version = await docker.System.GetVersionAsync();
            logger.LogInformation("Docker version: {Version}", version.Version);
        }
        catch (HttpRequestException e)
        {
            logger.LogCritical(e, "Docker is not available, please check your Docker host configuration");
            Environment.Exit(-1);
        }

        logger.LogDebug("Docker initialized");

        await asyncScope.DisposeAsync();
    }
}
