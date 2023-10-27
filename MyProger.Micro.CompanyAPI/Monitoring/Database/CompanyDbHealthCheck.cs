using System.Data.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyProger.Micro.CompanyAPI.Database;

namespace MyProger.Micro.CompanyAPI.Monitoring.Database;

public class CompanyDbHealthCheck : IHealthCheck
{
    private readonly CompanyDbContext _jobDbContext;

    public CompanyDbHealthCheck(CompanyDbContext jobDbContext)
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