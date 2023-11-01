namespace MyProger.Core.Models;

public class CounterConfiguration
{
    public string MetricName { get; }

    public string MetricDescription { get; }

    public CounterConfiguration(string metricName, string metricDescription)
    {
        MetricName = metricName;
        MetricDescription = metricDescription;
    }
}