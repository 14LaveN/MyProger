namespace MyProger.Micro.JobListAPI.Monitoring.Interfaces;

public interface ICounter : IMetricWithTimer
{
    void Increment(ref double value , params string[] labels);
}