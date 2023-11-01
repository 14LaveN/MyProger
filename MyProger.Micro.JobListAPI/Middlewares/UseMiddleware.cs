namespace MyProger.Micro.JobListAPI.Middlewares;

public static class UseMiddleware
{
    public static IApplicationBuilder UseCustomMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<PerformanceMiddleware>();
        app.UseMiddleware<ErrorMiddleware>(app.Logger);
        app.UseMiddleware<RequestLoggigMiddleware>(app.Logger);
        
        return app;
    }
}