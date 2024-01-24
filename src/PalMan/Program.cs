using Cocona;
using Microsoft.Extensions.DependencyInjection;
using PalMan.Commands;
using PalMan.Interfaces;
using PalMan.Services;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<IConfigurationManager, ConfigurationManager>();
builder.Services.AddSingleton<IAgentClient, AgentClient>();

var app = builder.Build();

app.AddCommands<RootCommands>();

app.Run();
