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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка схемы базы данных
            modelBuilder.HasDefaultSchema("dbo");

            // Настройка типа данных столбца
            modelBuilder.Entity<AppUser>()
                .Property(b => b.UserName)
                .HasColumnType("varchar(256)");

            modelBuilder.Entity<AppUser>()
                .HasIndex(b => b.Id);

            // Настройка внешнего ключа
            modelBuilder.Entity<AppUser>()
                .HasOne(b => b.Scopes);
        }

        public DbSet<ScopeEntity> Scopes { get; set; } = null!;
    }
