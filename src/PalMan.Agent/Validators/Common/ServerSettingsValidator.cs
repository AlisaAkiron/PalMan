using FluentValidation;
using PalMan.Agent.Attributes;
using PalMan.Agent.Entities;

namespace PalMan.Agent.Validators.Common;

[AssemblyScanIgnored]
public class ServerSettingsValidator : AbstractValidator<PalWorldSettings>
{
    public ServerSettingsValidator(List<PalWorldSettings> existingSettings)
    {
        RuleFor(x => x)
            .MustBeValidServerArguments(existingSettings);
    }
}
