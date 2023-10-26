using MyProger.Core.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Database;
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<long>, long>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ScopeEntity> Scopes { get; set; } = null!;
    }
