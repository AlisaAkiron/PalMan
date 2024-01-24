using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Database;

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

        await dbContext.DisposeAsync();

        await asyncScope.DisposeAsync();
    }
}
