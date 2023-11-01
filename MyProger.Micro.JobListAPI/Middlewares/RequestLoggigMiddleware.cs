namespace MyProger.Micro.JobListAPI.Middlewares;

public class RequestLoggigMiddleware
{
    public class RequestLoggingMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;
    
        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Записать запрос в журнал
            _logger.LogInformation("Get request: {Request}", context.Request);

            await context.RequestServices.GetRequiredService<RequestDelegate>().Invoke(context);
        }
    }
}