using Dapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyProger.Core.Entity.Likes;
using MyProger.Core.Models.MongoSettings;

namespace MyProger.Micro.JobListAPI.Database;

public class LikesContext
{
    private readonly IMongoDatabase database = null!;

    public LikesContext(IOptions<Settings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        if (client != null)
            database = client.GetDatabase(settings.Value.Database);
    }
    public IMongoCollection<LikeEntity> Likes
    {
        get => database.GetCollection<LikeEntity>("Likes");
    }
}