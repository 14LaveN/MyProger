using MyProger.Core.Entity.Likes;
using MyProger.Micro.JobListAPI.Database;
using MyProger.Micro.JobListAPI.Database.Interfaces;
using MyProger.Micro.JobListAPI.Database.Repositories;

namespace MyProger.Micro.JobListAPI.Common.Entry;

public static class EntryDatabase
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddTransient<JobDbContext>(provider => new JobDbContext());
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILikesRepository<LikeEntity>, LikesRepository>();
        
        return services;
    }
}