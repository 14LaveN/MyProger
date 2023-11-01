using System.Data.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyProger.Micro.JobListAPI.Database;

namespace MyProger.Micro.JobListAPI.Monitoring.Database;

public class JobDbHealthCheck : IHealthCheck
{
    private readonly JobDbContext _jobDbContext;
    
    public JobDbHealthCheck(JobDbContext jobDbContext)
    {
        _jobDbContext = jobDbContext;
    }


    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
    {
        var conn = _jobDbContext.CreateConnection();
        {
            try
            {
                conn.Open();
            }
            catch (DbException ex)
            {
                return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
            }
        }

        return HealthCheckResult.Healthy();
    }
}