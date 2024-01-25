using FluentValidation;
using PalMan.Agent.Database;
using PalMan.Shared.Models;
using PalMan.Shared.Models.Requests;

namespace PalMan.Agent.Validators;

public class UpdateServerRequestValidator : AbstractValidator<AgentRequest<UpdateServerRequest>>
{
    public UpdateServerRequestValidator(PalManDbContext dbContext)
    {
        RuleFor(x => x.Data.Identifier)
            .NotEmpty().WithMessage("Identifier is required")
            .Custom((s, context) =>
            {
                var existingServer = dbContext.PalWorldServers.FirstOrDefault(x => x.Identifier == s);
                if (existingServer is null)
                {
                    context.AddFailure("Server does not exist");
                    return;
                }

                if (existingServer.Installed is false)
                {
                    context.AddFailure("Server is not installed");
                }
            });
    }
}
