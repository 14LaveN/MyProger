using FluentValidation;
using MyProger.Mciro.SearchAPI.Command.Search.SearchJob;

namespace MyProger.Mciro.SearchAPI.Common.Entry;

public static class EntryValidator
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<IValidator<SearchJobCommand>, SearchJobCommandValidator>();
        
        return services;
    }
}