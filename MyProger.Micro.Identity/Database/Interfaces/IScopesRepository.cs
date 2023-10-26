using MyProger.Core.Entity;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Database.Interfaces;

public interface IScopesRepository
{
    Task<List<ScopeEntity>> GetAllScopes();

    Task<ScopeEntity> AddScopeToUser(long userId, string scopeName);
    
    Task RemoveScopeFromUser(long userId, string scopeName);

    Task<ScopeEntity> AddScope(ScopeEntity scopeEntity);
}