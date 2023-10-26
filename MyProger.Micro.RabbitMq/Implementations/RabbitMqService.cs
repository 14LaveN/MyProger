using System.Text;
using MyProger.Micro.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace MyProger.Micro.RabbitMQ.Implementations;

public class RabbitMqService :  IRabbitMqService
{
    public async Task SendMessage(string message, string rabbitName)
    {
        var factory = new ConnectionFactory() { Uri = new Uri("amqps://dgpswpjt:tbQvnOh93n-sdqDMjXAjfB53OiShmOka@chimpanzee.rmq.cloudamqp.com/dgpswpjt") };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateChannel();
        await channel.QueueDeclareAsync(queue: $"{rabbitName}-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: $"{rabbitName}-queue", exchange: $"{rabbitName}-exchange", routingKey: $"{rabbitName}-queue");
        
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: $"{rabbitName}-exchange",
            routingKey: $"{rabbitName}-queue",
            basicProperties: new BasicProperties(),
            body: body);
    }
}