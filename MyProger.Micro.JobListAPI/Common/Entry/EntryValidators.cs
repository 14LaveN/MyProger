using FluentValidation;
using MyProger.Micro.Identity.Database.Interfaces;
using MyProger.Micro.Identity.Database.Repository;
using MyProger.Micro.JobListAPI.Commands.AddJob;
using MyProger.Micro.JobListAPI.Commands.AddLikeToJob;
using MyProger.Micro.JobListAPI.Commands.CloseJob;
using MyProger.Micro.JobListAPI.Commands.DeleteJob;
using MyProger.Micro.JobListAPI.Commands.UpdateJob;

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
        services.AddScoped<IValidator<CloseJobCommand>, CloseJobCommandValidator>();
        services.AddScoped<IValidator<DeleteJobCommand>, DeleteJobCommandValidator>();
        services.AddScoped<IValidator<UpdateJobCommand>, UpdateJobCommandValidator>();
        services.AddScoped<IValidator<AddLikeToJobCommand>, AddLikeToJobCommandValidator>();

        return services;
    }
}