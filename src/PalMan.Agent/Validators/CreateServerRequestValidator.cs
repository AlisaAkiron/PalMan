using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PalMan.Agent.Database;
using PalMan.Agent.Extensions;
using PalMan.Agent.Validators.Common;
using PalMan.Shared.Models;
using PalMan.Shared.Models.Requests;

namespace PalMan.Agent.Validators;

public class CreateServerRequestValidator : AbstractValidator<AgentRequest<CreateServerRequest>>
{
    public CreateServerRequestValidator(PalManDbContext dbContext)
    {
        var currentServers = dbContext.PalWorldServers
            .AsNoTracking()
            .Include(x => x.Settings)
            .ToList();

        RuleFor(x => x.Data.Identifier)
            .NotEmpty().WithMessage("Identifier is required")
            .Must(x => x.IsValidServerIdentifier()).WithMessage("Identifier can only contain letters, numbers, underscores and dashes")
            .Must(x => currentServers.Exists(c => c.Identifier == x) is false).WithMessage("Identifier already exist");

        RuleFor(x => x.Data.ServerArguments)
            .MustBeValidServerArguments(currentServers.Select(x => x.Settings).ToList());
    }
}
