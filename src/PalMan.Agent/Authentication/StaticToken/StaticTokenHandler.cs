using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PalMan.Agent.Database;

namespace PalMan.Agent.Authentication.StaticToken;

public class StaticTokenHandler : AuthenticationHandler<StaticTokenOptions>
{
    private readonly PalManDbContext _dbContext;

    [Obsolete("Obsolete")]
    [SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed")]
    public StaticTokenHandler(
        PalManDbContext dbContext,
        IOptionsMonitor<StaticTokenOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _dbContext = dbContext;
    }

    public StaticTokenHandler(
        PalManDbContext dbContext,
        IOptionsMonitor<StaticTokenOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
        _dbContext = dbContext;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeader = Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return AuthenticateResult.Fail("Authorization header is not found.");
        }

        var token = authorizationHeader.Split(' ');
        if (token.Length != 2)
        {
            return AuthenticateResult.Fail("Invalid token format.");
        }

        var tokenSchema = token[0];
        if (tokenSchema.Equals("Bearer", StringComparison.OrdinalIgnoreCase) is false)
        {
            return AuthenticateResult.Fail("Invalid token schema.");
        }

        var tokenValue = token[1];
        if (string.IsNullOrEmpty(tokenValue))
        {
            return AuthenticateResult.Fail("Invalid token value.");
        }

        var existingToken = await _dbContext.Tokens
            .FirstOrDefaultAsync(x => x.TokenValue == tokenValue);
        if (existingToken is null)
        {
            return AuthenticateResult.Fail("Invalid token.");
        }

        var claims = new Claim[]
        {
            new("id", existingToken.Id.ToString()),
            new("name", existingToken.Name)
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
