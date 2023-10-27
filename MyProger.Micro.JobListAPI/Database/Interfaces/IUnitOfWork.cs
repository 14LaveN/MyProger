using MyProger.Core.Entity.Likes;

namespace MyProger.Micro.JobListAPI.Database.Interfaces;

public interface IUnitOfWork
{
    IJobRepository JobRepository { get; }

    ILikesRepository<LikeEntity> LikesRepository { get; }
}