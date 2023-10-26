using MyProger.Core.Entity;
using MyProger.Micro.Identity.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Database.Repository;

public class ScopesRepository 
    : IScopesRepository
{
    private readonly AppDbContext _appDbContext;

    public ScopesRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }


    public async Task<ScopeEntity> AddScope(ScopeEntity scopeEntity)
    {                   
        await _appDbContext.Scopes.AddAsync(scopeEntity);
        await _appDbContext.SaveChangesAsync();

        return scopeEntity;
    }

    public async Task<List<ScopeEntity>> GetAllScopes()
    {
        return await _appDbContext.Scopes.ToListAsync();
    }

    public async Task<ScopeEntity> AddScopeToUser(long userId, string scopeName)
    {
        var user = await _appDbContext.Users
            .Include(x => x.Scopes)
            .FirstOrDefaultAsync(x => x.Id == userId);
        var scope = await _appDbContext.Scopes
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Name == scopeName);

        if (user is not null)
        {
            user.Scopes ??= new List<ScopeEntity>();
            if (scope is not null) user.Scopes?.Add(scope);

            _appDbContext.Users.Update(user);
        }

        await _appDbContext.SaveChangesAsync();

        return scope;
    }

    public async Task RemoveScopeFromUser(long userId, string scopeName)
    {
        var user = await _appDbContext.Users
            .Include(x => x.Scopes)
            .FirstOrDefaultAsync(x => x.Id == userId);
        var scope = await _appDbContext.Scopes
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Name == scopeName);
        
        user.Scopes ??= new List<ScopeEntity>();
        user.Scopes?.Remove(scope);
        
        _appDbContext.Users.Update(user);
        await _appDbContext.SaveChangesAsync();
    }
}