namespace MyProger.Micro.Identity.Database.Interfaces;

public interface IUnitOfWork
{
    IAppUserRepository AppUserRepository { get; }

    IScopesRepository ScopesRepository { get; }
}