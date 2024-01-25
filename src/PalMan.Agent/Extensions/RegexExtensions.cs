using System.Text.RegularExpressions;

namespace PalMan.Agent.Extensions;

public static partial class RegexExtensions
{
    [GeneratedRegex("^[a-zA-Z0-9_-]+$")]
    private static partial Regex ValidServerIdentifier();

    [GeneratedRegex("^[a-zA-Z0-9]+$")]
    private static partial Regex ValidServerPassword();

    [GeneratedRegex("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$")]
    private static partial Regex ValidIPv4Address();

    public static bool IsValidServerIdentifier(this string identifier)
    {
        return ValidServerIdentifier().Match(identifier).Success;
    }

    public static bool IsValidServerPassword(this string password)
    {
        return ValidServerPassword().Match(password).Success;
    }

    public static bool IsIPv4Address(this string address)
    {
        return ValidIPv4Address().Match(address).Success;
    }
}
