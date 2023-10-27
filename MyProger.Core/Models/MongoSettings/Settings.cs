namespace MyProger.Core.Models.MongoSettings;

public class Settings
{
    public string ConnectionString { get; set; } = null!;

    public string Database { get; set; } = null!;
    
    public string LikesCollectionName { get; set; } = null!;
}