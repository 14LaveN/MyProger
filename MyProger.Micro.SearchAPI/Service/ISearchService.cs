using MyProger.Core.Entity.Job;

namespace MyProger.Mciro.SearchAPI.Service;

public interface ISearchService : IDisposable
{
    Task<List<JobEntity>> SearchProductsAsync(string query);
    
    Task DeleteJob(Guid id);
    
    Task<JobEntity> UpdateJob(Guid id, JobEntity jobEntity);
}