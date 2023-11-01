using System.Reflection;
using Prometheus;
using CounterConfiguration = MyProger.Core.Models.CounterConfiguration;
using IMetricsFactory = MyProger.Micro.JobListAPI.Monitoring.IMetricsFactory;
using ICounter = MyProger.Micro.JobListAPI.Monitoring.Interfaces.ICounter;
using PrometheusCounter = Prometheus.Counter;

namespace MyProger.Micro.JobListAPI.Monitoring.Implementations;

public class MetricsFactory : IMetricsFactory
{
    public ICounter CreateCounter(CounterConfiguration configuration)
    {
        var counterType = typeof(PrometheusCounter);
        var constructor = counterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string), typeof(string) }, null);
        var counter = (PrometheusCounter)constructor.Invoke(new object[] { configuration.MetricName, configuration.MetricDescription });

        return new Counters(counter);
    }

    public IGauge CreateGauge(GaugeConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}