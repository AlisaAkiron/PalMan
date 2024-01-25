using FluentValidation;
using PalMan.Agent.Entities;
using PalMan.Agent.Extensions;
using PalMan.Shared.Models.PalWorld;

namespace PalMan.Agent.Validators.Common;

public static class CommonRules
{
    public static IRuleBuilderOptions<T, ServerArguments> MustBeValidServerArguments<T>(this IRuleBuilder<T, ServerArguments> ruleBuilder, List<PalWorldSettings> existingSettings)
    {
        return ruleBuilder
            .ChildRules(c =>
            {
                c.RuleFor(x => x.AdminPassword)
                    .Must(p => p.IsValidServerPassword()).WithMessage("Admin password can only contain letters and numbers");

                c.RuleFor(x => x.RCONEnabled)
                    .Must(p => p).WithMessage("RCON must be enabled (This error is not possible to get)");

                c.RuleFor(x => x.ServerName)
                    .NotEmpty().WithMessage("Server name cannot be empty");

                c.RuleFor(x => x.ServerDescription)
                    .NotEmpty().WithMessage("Server description cannot be empty");

                c.RuleFor(x => x.PublicIP)
                    .Must(p => p.IsIPv4Address()).WithMessage("Public IP must be a valid IPv4 address");

                c.RuleFor(x => x.ServerPlayerMaxNum)
                    .InclusiveBetween(1, 32).WithMessage("Player max number must be between 1 and 32");

                c.RuleFor(x => x.GuildPlayerMaxNum)
                    .InclusiveBetween(1, 32).WithMessage("Guild player max number must be between 1 and 32");

                c.RuleFor(x => x.CoopPlayerMaxNum)
                    .InclusiveBetween(1, 32).WithMessage("Coop player max number must be between 1 and 32");

                c.RuleFor(x => x.DeathPenalty)
                    .Must(p => p is "None" or "Item" or "ItemAndEquipment" or "All");

                c.RuleFor(x => x.PublicPort)
                    .InclusiveBetween(1000, 65535).WithMessage("Public port must be between 1000 and 65535")
                    .Must(p => existingSettings.Exists(z => z.PublicPort == p) is false).WithMessage("Public port already exist");

                c.RuleFor(x => x.RCONPort)
                    .InclusiveBetween(1000, 65535).WithMessage("RCON port must be between 1000 and 65535")
                    .Must(p => existingSettings.Exists(z => z.RCONPort == p) is false).WithMessage("RCON port already exist");
            });
    }
}
