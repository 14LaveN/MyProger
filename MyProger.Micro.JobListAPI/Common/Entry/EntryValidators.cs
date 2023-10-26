using FluentValidation;
using MyProger.Micro.JobListAPI.Commands.AddJob;
using MyProger.Micro.JobListAPI.Database.Interfaces;
using MyProger.Micro.JobListAPI.Database.Repositories;

namespace MyProger.Micro.JobListAPI.Common.Entry;

public static class EntryValidators
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IValidator<AddJobCommand>, AddJobCommandValidator>();
        
        return services;
    }
}