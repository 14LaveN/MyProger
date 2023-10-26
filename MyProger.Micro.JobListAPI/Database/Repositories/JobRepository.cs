using Dapper;
using MyProger.Core.Entity.Job;
using MyProger.Micro.JobListAPI.Database.Interfaces;

namespace MyProger.Micro.JobListAPI.Database.Repositories;

public class JobRepository : IJobRepository
{
    private readonly JobDbContext _jobDbContext;

    public JobRepository(JobDbContext jobDbContext)
    {
        _jobDbContext = jobDbContext;
    }

    public async Task<IEnumerable<JobEntity>> GetJobsByCompany(string companyName)
    {
        using var conn = _jobDbContext.CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            var jobs = await conn.QueryAsync<JobEntity>("SELECT * FROM jobs WHERE CompanyName = @CompanyName", 
                new {CompanyName = companyName});
            transaction.Commit();
            return jobs;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<IEnumerable<JobEntity>> GetAllJobs()
    {
        using var conn = _jobDbContext.CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            var jobs = await conn.QueryAsync<JobEntity>("SELECT * FROM jobs");
            transaction.Commit();
            return jobs;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public async Task<JobEntity> GetJobByTitleAndCompany(string companyName, string title)
    {
        using var conn = _jobDbContext.CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            var jobs = await conn.QueryFirstOrDefaultAsync<JobEntity>("SELECT * FROM jobs " +
                                                                    "WHERE CompanyName = @CompanyName" +
                                                                    " AND Title = @Title", 
                new {CompanyName = companyName, Title = title});
            transaction.Commit();
            return jobs;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public async Task<JobEntity> CreateJob(JobEntity jobEntity)
    {
        using var conn = _jobDbContext.CreateConnection();
        
        conn.Open();
        
        using var transaction = conn.BeginTransaction();

        try
        {
            var job = await conn.QuerySingleOrDefaultAsync<JobEntity>(
            "INSERT INTO jobs (CompanyName, Title, CreationDate, IsClosed, ViewsCount, Description, Wage, PhoneNumber, LikesCount) " +
            "VALUES(@CompanyName, @Title, @CreationDate, @IsClosed, @ViewsCount, @Description, @Wage, @PhoneNumber, @LikesCount)", jobEntity);

            transaction.Commit();
            return job;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public async Task<JobEntity> UpdateJob(Guid id, JobEntity jobEntity)
    {
        using var conn = _jobDbContext.CreateConnection();

        conn.Open();
        
        using var transaction = conn.BeginTransaction();

        try
        {
            var job =  await conn.QueryFirstOrDefaultAsync<JobEntity>("UPDATE jobs SET CompanyName = @CompanyName, " +
                                                                        "ViewsCount = @ViewsCount, " +
                                                                        "Description = @Description, " +
                                                                        "CreationDate = @CreationDate, " +
                                                                        "IsClosed = @IsClosed," +
                                                                        " Title = @Title," +
                                                                        " LikesCount = @LikesCount," +
                                                                        " Wage = @Wage," +
                                                                        " PhoneNumber = @PhoneNumber  " +
                                                                        "WHERE Id = @Id", jobEntity );
            
            transaction.Commit();
            return job;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public async Task DeleteJob(Guid id)
    {
        using var conn = _jobDbContext.CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            await conn.QueryFirstOrDefaultAsync<JobEntity>("DELETE FROM jobs WHERE Id = @Id", new { Id = id });
            transaction.Commit();
        }
        catch (Exception) 
        { 
            transaction.Rollback();
            throw; 
        }
    }

    public async Task<JobEntity> GetById(Guid id)
    {
        using var conn = _jobDbContext.CreateConnection();
        conn.Open();
        
        using var transaction = conn.BeginTransaction();

        try
        {
            var job = await conn.QueryFirstOrDefaultAsync<JobEntity>("SELECT * FROM jobs WHERE Id = @Id", 
                new { Id = id });
            transaction.Commit();
            return job;
        }
        catch (Exception) 
        { 
            transaction.Rollback();
            throw; 
        }
    }
}