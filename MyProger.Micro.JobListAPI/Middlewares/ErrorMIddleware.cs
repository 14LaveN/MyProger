using System.Text.Json;

namespace MyProger.Micro.JobListAPI.Middlewares;

public class ErrorMiddleware
{
    private readonly ILogger<ErrorMiddleware> _logger;

    public ErrorMiddleware(ILogger<ErrorMiddleware> logger)
    {
        _logger = logger;
    }

    public void Invoke(HttpContext context)
    {
        try
        {
            // Обрабатываем запрос
            context.Response.WriteAsync("Hello, world!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[MiddlewareHandlingException] Error: {ex.Message}, StackTrace: {ex.StackTrace}");
            var message = JsonSerializer.Serialize(new
            {
                Message = ex.Message
            });
            
            context.Response.WriteAsync(message);
            // Сохраняем ошибку

            // Отправляем пользователю сообщение об ошибке
            context.Response.StatusCode = 500;
            context.Response.WriteAsync("Internal server error");
        }
    }
}