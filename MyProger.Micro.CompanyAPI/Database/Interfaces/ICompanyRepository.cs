using MyProger.Core.Entity.Company;

namespace MyProger.Micro.CompanyAPI.Database.Interfaces;

public interface ICompanyRepository
{
    Task<CompanyEntity> GetById(Guid id);
    
    Task<CompanyEntity> CreateCompany(CompanyEntity companyName);

    Task DeleteCompany(Guid id);

    Task<CompanyEntity> UpdateCompany(Guid id, CompanyEntity companyEntity);

    Task<CompanyEntity> GetCompanyByName(string companyName);

    Task<IEnumerable<CompanyEntity>> GetAllCompanies();
}