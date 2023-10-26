namespace MyProger.Micro.RabbitMQ.Interfaces;

public interface IRabbitMqService
{
    Task SendMessage(string message, string rabbitName);
}