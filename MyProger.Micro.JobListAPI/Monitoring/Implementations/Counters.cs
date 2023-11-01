using Prometheus;
using ICounter = MyProger.Micro.JobListAPI.Monitoring.Interfaces.ICounter;

namespace MyProger.Micro.JobListAPI.Monitoring.Implementations;

public class Counters : ICounter
{
    private readonly Counter _counter;

    public Counters(Counter counter)
    {
        _counter = counter;
    }

    public IDisposable CreateTimer(params string[] labels) => _counter.NewTimer();

    public void Increment(ref double value, params string[] labels) => _counter.WithLabels(labels).Inc(value);
}