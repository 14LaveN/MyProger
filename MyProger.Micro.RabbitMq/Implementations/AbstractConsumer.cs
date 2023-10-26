using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyProger.Micro.RabbitMQ.Implementations;

public class AbstractConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _rabbitName;

    public AbstractConsumer(string rabbitName)
    {
        // Не забудьте вынести значения "localhost" и "MyQueue"
        // в файл конфигурации
        _rabbitName = rabbitName;
        var factory = new ConnectionFactory { Uri = new Uri("amqps://dgpswpjt:tbQvnOh93n-sdqDMjXAjfB53OiShmOka@chimpanzee.rmq.cloudamqp.com/dgpswpjt")};
        _connection = factory.CreateConnection();
        _channel = _connection.CreateChannel();
        _channel.ExchangeDeclare(exchange: $"{rabbitName}-exchange", type: ExchangeType.Topic, durable: true);
        _channel.QueueDeclare(queue: $"{rabbitName}-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queue: $"{rabbitName}-queue", exchange: $"{rabbitName}-exchange", routingKey: $"{rabbitName}-queue");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
			
            // Каким-то образом обрабатываем полученное сообщение
            Console.WriteLine(content);
            Debug.WriteLine($"Message recieved: {content}");

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume($"{_rabbitName}-queue", false, consumer);

        return Task.CompletedTask;
    }
	
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}