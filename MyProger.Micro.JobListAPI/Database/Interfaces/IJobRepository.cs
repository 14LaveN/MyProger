using MyProger.Core.Entity.Job;

namespace MyProger.Micro.JobListAPI.Database.Interfaces;

public interface IJobRepository
{
    Task<JobEntity> GetById(Guid id);
    
    Task<JobEntity> CreateJob(JobEntity jobEntity);

    Task DeleteJob(Guid id);

    Task<JobEntity> UpdateJob(Guid id, JobEntity jobEntity);

    Task<JobEntity> GetJobByTitleAndCompany(string companyName, string title);
    
    Task<IEnumerable<JobEntity>> GetJobsByCompany(string companyName);

    Task<IEnumerable<JobEntity>> GetAllJobs();
}