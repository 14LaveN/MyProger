using MyProger.Micro.CompanyAPI.Database;
using MyProger.Micro.CompanyAPI.Database.Interfaces;
using MyProger.Micro.CompanyAPI.Database.Repository;

namespace MyProger.Micro.CompanyAPI.Common.Entry;

public static class EntryDatabase
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddTransient<CompanyDbContext>(provider => new CompanyDbContext());
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}