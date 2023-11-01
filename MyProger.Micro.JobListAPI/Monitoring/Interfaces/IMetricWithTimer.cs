namespace MyProger.Micro.JobListAPI.Monitoring.Interfaces;

public interface IMetricWithTimer
{
    IDisposable CreateTimer(params string[] labels);
}