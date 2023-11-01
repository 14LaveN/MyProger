using MyProger.Micro.JobListAPI.Monitoring;
using MyProger.Micro.JobListAPI.Monitoring.Implementations;
using Prometheus;
using ICounter = MyProger.Micro.JobListAPI.Monitoring.Interfaces.ICounter;

namespace MyProger.Micro.JobListAPI.Common.Entry;

public static class EntryMonitoring
{
    public static IServiceCollection AddMonitoring(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<IMetricsFactory, MetricsFactory>();
        services.AddScoped<ICounter, Counters>();
        services.AddTransient<Counter>();
        
        return services;
    }
}