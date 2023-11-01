using FluentValidation;
using MyProger.Micro.CompanyAPI.Command.Company.CreateCompany;
using MyProger.Micro.CompanyAPI.Command.Company.DeleteCompany;

namespace MyProger.Micro.CompanyAPI.Common.Entry;

public static class EntryValidators
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IValidator<CreateCompanyCommand>, CreateCompanyCommandValidator>();
        services.AddScoped<IValidator<DeleteCompanyCommand>, DeleteCompanyCommandValidator>();

        return services;
    }
}