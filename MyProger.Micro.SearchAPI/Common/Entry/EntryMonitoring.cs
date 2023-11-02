using MyProger.Mciro.SearchAPI.Metrics.Request;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MyProger.Mciro.SearchAPI.Common.Entry;

public static class EntryMonitoring
{
    public static IServiceCollection AddMonitoring(this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var tracingOtlpEndpoint = "https://localhost:7120/prometheus/";

        services.AddTransient<RequestMetrics>(_ => new RequestMetrics());

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName: environment.ApplicationName))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter()
                .AddHttpClientInstrumentation()
                .AddMeter("Metrics.NET")
                .AddMeter("Microsoft.AspNetCore.Hosting")
                .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                .AddPrometheusExporter())
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                tracing.AddSource();
                tracing.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
                });
            });
        
        return services;
    }
}