using Docker.DotNet;
using Docker.DotNet.BasicAuth;
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

var app = builder.Build();

await app.InitializeDatabase();

app.MapControllers();

app.Run();
