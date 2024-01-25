using System.Diagnostics.CodeAnalysis;
using System.Text;
using PalMan.Shared.Models.PalWorld;

namespace PalMan.Shared.Utils;

[SuppressMessage("Globalization", "CA1305:Specify IFormatProvider")]
public static class ServerArgumentsSerializer
{
    public static string Serialize(ServerArguments arguments)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("[/Script/Pal.PalGameWorldSettings]");
        stringBuilder.Append("OptionSettings=(");

        var properties = typeof(ServerArguments).GetProperties();
        foreach (var property in properties)
        {
            var key = property.Name;
            var value = property.GetValue(arguments)!;

            var formattedValue = FormatValue(value, property.Name);

            stringBuilder.Append($"{key}=${formattedValue},");
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        stringBuilder.Append(')');
        stringBuilder.AppendLine();

        return stringBuilder.ToString();
    }

    public static string FormatValue(object o, string propertyName)
    {
        return o switch
        {
            bool boolValue => boolValue ? "True" : "False",
            float floatValue => floatValue.ToString("F6"),
            int intValue => intValue.ToString(),
            string stringValue when string.IsNullOrEmpty(stringValue) => "\"\"",
            string stringValue when stringValue.Contains(' ') => $"\"{stringValue}\"",
            string stringValue when propertyName == "BanListURL" => $"\"{stringValue}\"",
            string stringValue => stringValue,
            _ => throw new InvalidOperationException("Unknown type")
        };
    }
}
