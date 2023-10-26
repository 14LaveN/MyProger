namespace MyProger.Micro.Identity.Models.Identity;

public class RefreshTokenData
{
    public string Key { get; set; } = null!;
    public DateTime Expire { get; set; }
    public long UserId { get; set; }
}