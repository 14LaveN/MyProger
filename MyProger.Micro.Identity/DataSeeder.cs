using MyProger.Core.Entity.Account;
using MyProger.Micro.Identity.Database;
using MyProger.Micro.Identity.Models.Identity;

namespace MyProger.Micro.Identity;

public class DataSeeder
{
    private readonly AppDbContext _context;

    public DataSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task Execute()
    {
        if (_context.Scopes.Any())
        {
            return;
        }

        var scopes = new[]
        {
            Scope.ProfileRead,
            Scope.ProfileAllRead,
            Scope.ProfileWrite,
            Scope.ProfileAllWrite,
            Scope.ScopesRead,
            Scope.ScopesAllRead,
            Scope.ScopesWrite,
            Scope.ScopesAllWrite
        };
        
        await _context.Scopes.AddRangeAsync(scopes.Select(x => new ScopeEntity(x)));
        await _context.SaveChangesAsync();
    }
}