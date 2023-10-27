using MyProger.Micro.CompanyAPI.Database.Interfaces;

namespace MyProger.Micro.CompanyAPI.Database.Repository;

public class UnitOfWork
    : IUnitOfWork
{
    private ICompanyRepository? _companyRepository;
    private readonly CompanyDbContext _companyDbContext;

    public UnitOfWork(ICompanyRepository? companyRepository,
        CompanyDbContext companyDbContext)
    {
        _companyRepository = companyRepository;
        _companyDbContext = companyDbContext;
    }

    public ICompanyRepository CompanyRepository
    {
        get
        {
            if (_companyRepository is null)
                _companyRepository = new CompanyRepository(_companyDbContext);
            return _companyRepository;
        }
    }
}