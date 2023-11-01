using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Identity.Client;
using Metrics = Prometheus.Metrics;

namespace MyProger.Micro.JobListAPI.Middlewares;

public class PerformanceMiddleware
{
    public void Invoke(HttpContext context)
    {
        // Измеряем время начала запроса
        long startTime = DateTime.Now.Ticks;

        // Обрабатываем запрос
        context.Response.WriteAsync("Hello, world!");

        // Измеряем время окончания запроса
        long endTime = DateTime.Now.Ticks;

        var time = (endTime - startTime) / 10000000;
        
        // Сохраняем время отклика
        Metrics.CreateHistogram("RequestTime", "", time.ToString());
    }
}