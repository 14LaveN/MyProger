using Microsoft.AspNetCore.Identity;

namespace MyProger.Core.Entity.Account;

public class AppUser : IdentityUser<long>
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Patronymic { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? RefreshToken { get; set; }
    public ICollection<ScopeEntity>? Scopes { get; set; }
}