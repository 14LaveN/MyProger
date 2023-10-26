using Microsoft.Extensions.DependencyInjection;
using MyProger.Micro.RabbitMQ.Implementations;
using MyProger.Micro.RabbitMQ.Interfaces;

namespace MyProger.Micro.RabbitMQ;

public static class EntryRabbitMq
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, string rabbitName)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddHostedService<AbstractConsumer>(provider => new AbstractConsumer(rabbitName));
        services.AddScoped<IRabbitMqService, RabbitMqService>();
        
        return services;
    }
}