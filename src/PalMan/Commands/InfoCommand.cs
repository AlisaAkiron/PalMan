using System.ComponentModel;
using System.Reflection;
using Cocona;
using PalMan.Shared.Models.PalWorld;
using PalMan.Shared.Utils;
using Spectre.Console;

namespace PalMan.Commands;

public class InfoCommand
{
    [Command("settings", Description = "Show all available PalWorldSettings.ini entries")]
    public void SettingsCommand()
    {
        var properties = typeof(ServerArguments).GetProperties();
        var defaultArguments = new ServerArguments();

        var table = new Table();

        table.AddColumns("Name", "Type", "Default", "Description");

        foreach (var property in properties)
        {
            var description = property.GetCustomAttribute<DescriptionAttribute>()?.Description
                              ?? "[red]No description available[/]";

            if (description.StartsWith("[N]", StringComparison.InvariantCulture))
            {
                description = $"[yellow][[N]][/]{description[3..]}";
            }

            var propertyColorPrefix = property.PropertyType switch
            {
                { } t when t == typeof(bool) => "[green]",
                { } t when t == typeof(int) => "[blue]",
                { } t when t == typeof(string) => "[darkorange]",
                { } t when t == typeof(float) => "[aqua]",
                _ => "[red]"
            };

            var defaultValue = ServerArgumentsSerializer
                .FormatValue(property.GetValue(defaultArguments)!, property.Name);

            table.AddRow(
                property.Name,
                $"{propertyColorPrefix}{property.PropertyType.Name}[/]",
                defaultValue,
                description);
        }

        AnsiConsole.MarkupLine("Note: [yellow][[N]][/] means that this setting is not documented on the official website");
        AnsiConsole.Write(table);
    }
}
