using MyProger.Micro.JobListAPI.Database.Interfaces;

namespace MyProger.Micro.JobListAPI.Database.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IJobRepository? _jobRepository;
    private readonly JobDbContext _jobDbContext;

    public UnitOfWork(IJobRepository jobRepository, JobDbContext jobDbContext)
    {
        _jobRepository = jobRepository;
        _jobDbContext = jobDbContext;
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
}