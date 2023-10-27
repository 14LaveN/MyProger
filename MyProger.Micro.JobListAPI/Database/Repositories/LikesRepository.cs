using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyProger.Core.Entity.Likes;
using MyProger.Core.Models.MongoSettings;
using MyProger.Micro.JobListAPI.Database.Interfaces;

namespace MyProger.Micro.JobListAPI.Database.Repositories;

public class LikesRepository
    : ILikesRepository<LikeEntity>
{
    private readonly IMongoCollection<LikeEntity> _likesCollection;

    public LikesRepository(
        IOptions<Settings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.Database);

        _likesCollection = mongoDatabase.GetCollection<LikeEntity>(
            dbSettings.Value.LikesCollectionName);
    }

    public async Task<List<LikeEntity>> GetLikesByJobIdAsync(string jobId) =>
        await _likesCollection.FindAsync(x => x.JobId == jobId)
            .Result.ToListAsync();

    public async Task<List<LikeEntity>> GetAllAsync() =>
        await _likesCollection.Find(_ => true).ToListAsync();

    public async Task<LikeEntity?> GetAsync(string id) =>
        await _likesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<LikeEntity?> GetUserNameAsync(string userName) =>
        await _likesCollection.Find(x => x.UserName == userName).FirstOrDefaultAsync();

    public async Task CreateAsync(LikeEntity like) =>
        await _likesCollection.InsertOneAsync(like);

    public async Task RemoveAsync(string id) =>
        await _likesCollection.DeleteOneAsync(x => x.Id == id);
}