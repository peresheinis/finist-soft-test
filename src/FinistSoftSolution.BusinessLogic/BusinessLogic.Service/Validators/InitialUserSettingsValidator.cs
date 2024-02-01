using BusinessLogic.Service.Configurations;
using FluentValidation;

namespace BusinessLogic.Service.Validators;

public class InitialUserSettingsValidator : AbstractValidator<InitialUserSettings>
{
    public InitialUserSettingsValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage($"Please configure phone number in {nameof(InitialUserSettings)}!");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage($"Please configure password in {nameof(InitialUserSettings)}!");
    }
}