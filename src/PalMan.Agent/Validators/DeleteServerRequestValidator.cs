using FluentValidation;
using PalMan.Agent.Database;
using PalMan.Shared.Models.Requests;

namespace PalMan.Agent.Validators;

public class DeleteServerRequestValidator : AbstractValidator<DeleteServerRequest>
{
    public DeleteServerRequestValidator(PalManDbContext dbContext)
    {
        RuleFor(x => x.Identifier)
            .Must(x => dbContext.PalWorldServers.Any(c => c.Identifier == x))
            .WithMessage("Server does not exist");
    }
}
