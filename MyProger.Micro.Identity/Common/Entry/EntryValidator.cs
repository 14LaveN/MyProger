using MyProger.Micro.Identity.Commands.Login;
using MyProger.Micro.Identity.Commands.Register;
using FluentValidation;

namespace MyProger.Micro.Identity.Common.Entry;

public static class EntryValidator
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
        services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
        
        return services;
    }
}