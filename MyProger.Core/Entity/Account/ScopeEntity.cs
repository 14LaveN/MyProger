namespace MyProger.Core.Entity.Account;

public class ScopeEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<AppUser>? Users { get; set; }

    public ScopeEntity()
    {
    }

    public ScopeEntity(string name)
    {
        Name = name;
    }
    
    public static implicit operator ScopeEntity(string scopeInput)
    {
        return new ScopeEntity()
        {
            Name = scopeInput
        };
    }
}