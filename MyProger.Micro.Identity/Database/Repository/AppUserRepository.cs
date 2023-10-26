using MyProger.Core.Entity;
using MyProger.Micro.Identity.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Database.Repository;

public class AppUserRepository : IAppUserRepository
{
    private readonly AppDbContext _appDbContext;

    public AppUserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task SaveChanges()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<AppUser> GetById(long id)
    {
        return (await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id))!;
    }

    public async Task<AppUser> GetByName(string name)
    {
        return (await _appDbContext.Users
            .Include(x=>x.Scopes)
            .FirstOrDefaultAsync(x => x.UserName == name))!;
    }

    public async Task<List<AppUser>> GetAll()
    {
        return await _appDbContext.Users.ToListAsync();
    }
}