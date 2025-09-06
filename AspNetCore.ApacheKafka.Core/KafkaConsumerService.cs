using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AspNetCore.ApacheKafka.Core;

public class KafkaConsumerService<T> : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IKafkaConsumerHandler<T> _handler;

    private readonly ILogger Logger;

    public KafkaConsumerService(
        IOptions<KafkaSettings> settings,
        IKafkaConsumerHandler<T> handler,
        ILogger<KafkaConsumerService<T>> logger,
        string topic,
        string groupId,
        string autoOffsetReset)
    {
        _handler = handler;

        var config = new ConsumerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(autoOffsetReset, true)
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe(topic);

        Logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _consumer.Consume(stoppingToken); // blocking call

                    if (cr?.Message?.Value != null)
                    {
                        var message = JsonSerializer.Deserialize<T>(cr.Message.Value);
                        if (message != null)
                        {
                            await _handler.HandleAsync(message, stoppingToken); // properly awaited
                        }
                    }
                }
                catch (ConsumeException ex)
                {
                    Logger.LogError(ex, $"❌ Kafka error: {ex.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    break; // graceful shutdown
                }
            }
        }, stoppingToken);
    }
}