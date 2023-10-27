using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyProger.Core.Entity.Likes;

public class LikeEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? JobId { get; set; }

    public string UserName { get; set; }
}