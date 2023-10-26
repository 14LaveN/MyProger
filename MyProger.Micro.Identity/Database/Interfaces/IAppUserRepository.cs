using MyProger.Core.Entity;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Database.Interfaces;

public interface IAppUserRepository
{
    Task SaveChanges();

    Task<AppUser> GetById(long id);

    Task<AppUser> GetByName(string name);

    Task<List<AppUser>> GetAll();
}