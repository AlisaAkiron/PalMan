using Microsoft.AspNetCore.Authentication;
using PalMan.Agent.Authentication.StaticToken;

namespace PalMan.Agent.Authentication;

public static class AuthenticatorExtensions
{
    public static AuthenticationBuilder AddStaticToken(
        this AuthenticationBuilder builder,
        string authenticationSchema = "Static Token",
        string? displayName = null,
        Action<StaticTokenOptions>? configureOptions = null)
    {
        builder.AddScheme<StaticTokenOptions, StaticTokenHandler>(authenticationSchema, displayName, configureOptions);

        return builder;
    }
}
