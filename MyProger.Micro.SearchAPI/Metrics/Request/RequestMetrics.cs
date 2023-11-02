using System.Diagnostics;
using Metric = Prometheus.Client.Metrics;
namespace MyProger.Mciro.SearchAPI.Metrics.Request;

public class RequestMetrics
{
    private readonly Stopwatch _stopwatch = new Stopwatch();

    public void OnRequestReceived()
    {
        _stopwatch.Start();
    }

    public void OnRequestCompleted()
    {
        _stopwatch.Stop();
        var counter = Metric.DefaultFactory.CreateCounter("http_request_response_time",
            "The time it takes to process an HTTP request");
        counter.Inc(_stopwatch.ElapsedMilliseconds);
    }
}