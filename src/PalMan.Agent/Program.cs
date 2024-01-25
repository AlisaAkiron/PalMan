using System.Reflection;
using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using FluentValidation;
using FluentValidation.AspNetCore;
using PalMan.Agent.Attributes;
using PalMan.Agent.Authentication;
using PalMan.Agent.Authentication.StaticToken;
using PalMan.Agent.Constants;
using PalMan.Agent.Database;
using PalMan.Agent.Extensions;
using PalMan.Agent.Utils;
using Serilog;

Log.Logger = LoggerUtils.CreateLogger();

Log.Logger.Information("Starting PalMan Agent...");
LoggerUtils.PrintAgentConfiguration();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddDbContext<PalManDbContext>();
builder.Services.AddSingleton<IDockerClient, DockerClient>(_ =>
{
    var dockerUri = new Uri(DockerConfiguration.DockerHost);

    Credentials credential;

    if (string.IsNullOrEmpty(DockerConfiguration.DockerUsername) && string.IsNullOrEmpty(DockerConfiguration.DockerPassword))
    {
        credential = new AnonymousCredentials();
    }
    else
    {
        credential = new BasicAuthCredentials(DockerConfiguration.DockerUsername, DockerConfiguration.DockerPassword);
    }

    return new DockerClientConfiguration(dockerUri, credential).CreateClient();
});

builder.Services.AddValidatorsFromAssembly(
    typeof(Program).Assembly,
    filter: result => result.ValidatorType.GetCustomAttribute<AssemblyScanIgnoredAttribute>() is null);
builder.Services.AddFluentValidationAutoValidation(configure =>
{
    configure.DisableDataAnnotationsValidation = true;
});

builder.Services
    .AddAuthentication(StaticTokenDefaults.AuthenticationSchema)
    .AddStaticToken();

var app = builder.Build();

await app.InitializeDatabase();
await app.InitializeDocker();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
