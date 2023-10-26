using MyProger.Micro.Identity.Database.Interfaces;
using MyProger.Micro.Identity.Database.Repository;

namespace MyProger.Micro.Identity.Common.Entry;

public static class EntryDatabase
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IScopesRepository, ScopesRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}