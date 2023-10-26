using MyProger.Micro.Identity.Database.Interfaces;

namespace MyProger.Micro.Identity.Database.Repository;

public class UnitOfWork : IUnitOfWork
{
    private IAppUserRepository? _appUserRepository;
    private IScopesRepository? _scopesRepository;
    private readonly AppDbContext _accountDbContext;

    public UnitOfWork(IAppUserRepository appUserRepository,
        AppDbContext accountDbContext, IScopesRepository? scopesRepository)
    {
        _scopesRepository = scopesRepository;
        (_accountDbContext, _appUserRepository) =
            (accountDbContext, appUserRepository);
    }

    public IAppUserRepository AppUserRepository
    {
        get
        {
            if (_appUserRepository is null)
                _appUserRepository = new AppUserRepository(_accountDbContext);
            return _appUserRepository;
        }
    }
    
    public IScopesRepository ScopesRepository
    {
        get
        {
            if (_scopesRepository is null)
                _scopesRepository = new ScopesRepository(_accountDbContext);
            return _scopesRepository;
        }
    }
}