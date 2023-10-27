namespace MyProger.Micro.JobListAPI.Database.Interfaces;

public interface ILikesRepository<T>
{
    Task<List<T>> GetAllAsync();
    
    Task<List<T>> GetLikesByJobIdAsync(string jobId);

    Task<T?> GetAsync(string id);

    Task<T?> GetUserNameAsync(string userName);

    Task CreateAsync(T newLike);

    Task RemoveAsync(string id);
}