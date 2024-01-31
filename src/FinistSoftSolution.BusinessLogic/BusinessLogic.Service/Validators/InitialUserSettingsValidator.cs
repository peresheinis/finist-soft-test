using BusinessLogic.Service.Configurations;
using FluentValidation;

namespace BusinessLogic.Service.Validators;

public class InitialUserSettingsValidator : AbstractValidator<InitialUserSettings>
{
    public InitialUserSettingsValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Please configure phone number of initial user!");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Please configure password of initial user!");
    }
}