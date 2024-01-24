using System.Security.Cryptography;

namespace PalMan.Shared.Utils;

public static class RandomUtils
{
    private static ReadOnlySpan<char> PasswordCharacters =>
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".AsSpan();

    public static string GeneratePassword(int length = 32)
    {
        return RandomNumberGenerator.GetString(PasswordCharacters, length);
    }
}
