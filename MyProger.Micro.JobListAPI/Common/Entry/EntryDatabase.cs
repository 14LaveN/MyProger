using Microsoft.Extensions.Options;
using MyProger.Core.Entity.Likes;
using MyProger.Mciro.JobListAPI.Common;
using MyProger.Micro.JobListAPI.Database;
using MyProger.Micro.JobListAPI.Database.Interfaces;
using MyProger.Micro.JobListAPI.Database.Repositories;
using Nest;

namespace MyProger.Micro.JobListAPI.Common.Entry;

public static class EntryDatabase
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddDistributedMemoryCache();
        services.AddStackExchangeRedisCache(options =>
        {
            var connection = configuration.GetConnectionString("Redis");
            options.Configuration = connection;
        });

        services.AddSingleton<IElasticClient>(new ElasticClient(new Uri("https://localhost:9200")));
        services.AddTransient<ElasticsearchIndexer>();
        services.AddTransient<JobDbContext>(provider => new JobDbContext());
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILikesRepository<LikeEntity>, LikesRepository>();
        
        return services;
    }
}