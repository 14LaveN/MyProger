using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyProger.Core.Entity.Likes;
using MyProger.Core.Models.MongoSettings;
using MyProger.Micro.JobListAPI.Database.Interfaces;

namespace MyProger.Micro.JobListAPI.Database.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IJobRepository? _jobRepository;
    private ILikesRepository<LikeEntity>? _likesRepository;
    private readonly JobDbContext _jobDbContext;
    private readonly IOptions<Settings> _options;

    public UnitOfWork(IJobRepository jobRepository,
        JobDbContext jobDbContext,
        ILikesRepository<LikeEntity>? likesRepository,
        IOptions<Settings> options)
    {
        _jobRepository = jobRepository;
        _jobDbContext = jobDbContext;
        _likesRepository = likesRepository;
        _options = options;
    }

    public IJobRepository JobRepository
    {
        get
        {
            if (_jobRepository is null)
                _jobRepository = new JobRepository(_jobDbContext);
            return _jobRepository;
        }
    }
    
    public ILikesRepository<LikeEntity> LikesRepository
    {
        get
        {
            if (_likesRepository is null)
                _likesRepository = new LikesRepository(_options);
            return _likesRepository;
        }
    }
}