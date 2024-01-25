using System.Reflection;
using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using FluentValidation;
using FluentValidation.AspNetCore;
using PalMan.Agent.Attributes;
using PalMan.Agent.Authentication;
using PalMan.Agent.Authentication.StaticToken;
using PalMan.Agent.Database;
using PalMan.Agent.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddPalManConfigurations();

builder.Services.AddControllers();
builder.Services.AddDbContext<PalManDbContext>();
builder.Services.AddSingleton<IDockerClient, DockerClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var dockerHost = config.GetValue<string>("Docker:Host")!;
    var dockerUsername = config.GetValue<string>("Docker:Username")!;
    var dockerPassword = config.GetValue<string>("Docker:Password")!;

    var dockerUri = new Uri(dockerHost);

    Credentials credential;

    if (string.IsNullOrEmpty(dockerUsername) && string.IsNullOrEmpty(dockerPassword))
    {
        credential = new AnonymousCredentials();
    }
    else
    {
        credential = new BasicAuthCredentials(dockerUsername, dockerPassword);
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
