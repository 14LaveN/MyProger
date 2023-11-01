using Prometheus;
using CounterConfiguration = MyProger.Core.Models.CounterConfiguration;
using ICounter = MyProger.Micro.JobListAPI.Monitoring.Interfaces.ICounter;

namespace MyProger.Micro.JobListAPI.Monitoring;

public interface IMetricsFactory
{
    ICounter CreateCounter(CounterConfiguration configuration);

    IGauge CreateGauge(GaugeConfiguration configuration);
}