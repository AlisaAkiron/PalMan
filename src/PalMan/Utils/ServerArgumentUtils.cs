using PalMan.Shared.Models.PalWorld;
using Spectre.Console;

namespace PalMan.Utils;

public static class ServerArgumentUtils
{
    public static ServerArguments? Parse(IEnumerable<string> arguments)
    {
        var success = true;
        var serverArguments = new ServerArguments();

        var serverArgumentProperties = typeof(ServerArguments).GetProperties();

        foreach (var argument in arguments)
        {
            var split = argument.Split('=');
            if (split.Length != 2)
            {
                AnsiConsole.WriteLine($"[red]Invalid server argument, syntax error: {argument}[/]");
                success = false;
                continue;
            }

            var key = split[0];
            var value = split[1];

            var property = serverArgumentProperties.FirstOrDefault(x => x.Name == key);
            if (property is null)
            {
                AnsiConsole.WriteLine($"[red]Invalid server argument, unknown property: {argument}[/]");
                success = false;
                continue;
            }

            if (property.PropertyType == typeof(string))
            {
                property.SetValue(serverArguments, value);
            }
            else if (property.PropertyType == typeof(float))
            {
                if (!float.TryParse(value, out var intValue))
                {
                    AnsiConsole.WriteLine($"[red]Invalid server argument, invalid float: {argument}[/]");
                    success = false;
                    continue;
                }

                property.SetValue(serverArguments, intValue);
            }
            else if (property.PropertyType == typeof(int))
            {
                if (!int.TryParse(value, out var intValue))
                {
                    AnsiConsole.WriteLine($"[red]Invalid server argument, invalid integer: {argument}[/]");
                    success = false;
                    continue;
                }

                property.SetValue(serverArguments, intValue);
            }
            else if (property.PropertyType == typeof(bool))
            {
                if (!bool.TryParse(value, out var boolValue))
                {
                    AnsiConsole.WriteLine($"[red]Invalid server argument, invalid boolean: {argument}[/]");
                    success = false;
                    continue;
                }

                property.SetValue(serverArguments, boolValue);
            }
            else
            {
                AnsiConsole.WriteLine($"[red]Invalid server argument, unknown property type: {argument}[/]");
                success = false;
            }
        }

        return success ? serverArguments : null;
    }

    public static IEnumerable<string> GetChangeList(ServerArguments arguments)
    {
        var defaultArguments = new ServerArguments();
        var serverArgumentProperties = typeof(ServerArguments).GetProperties();

        foreach (var property in serverArgumentProperties)
        {
            var defaultValue = property.GetValue(defaultArguments);
            var value = property.GetValue(arguments);

            if (defaultValue is null || value is null)
            {
                continue;
            }

            if (defaultValue.Equals(value))
            {
                continue;
            }

            yield return property.Name;
        }
    }
}
