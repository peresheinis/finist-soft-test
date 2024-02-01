using BusinessLogic.Service.Configurations;
using FluentValidation;

namespace BusinessLogic.Service.Validators;

public class AuthorizationSettingsValidator : AbstractValidator<AuthorizationSettings>
{
    public AuthorizationSettingsValidator()
    {
        RuleFor(x => x.Issuer)
            .NotEmpty()
            .WithMessage($"Please configure Issuer in {nameof(AuthorizationSettings)}");

        RuleFor(x => x.ExpiresInMinutes)
            .NotEmpty()
            .WithMessage($"Please configure ExpiresInMinutes in {nameof(AuthorizationSettings)}");
    }
}