using BusinessLogic.Service.Validators;
using Microsoft.Extensions.Options;

namespace BusinessLogic.Service.Extensions;

public static class OptionsBuilderFluentValidationExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(
      this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
            provider => new FluentValidationOptions<TOptions>(
              optionsBuilder.Name, provider));
        return optionsBuilder;
    }
}