using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AspNetCore.ApacheKafka.Core;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger Logger;

    public KafkaProducer(IOptions<KafkaSettings> settings, ILogger<KafkaProducer> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers,
            ClientId = settings.Value.Producer.ClientId
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();

        Logger = logger;
    }

    public async Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(message);

        var result = await _producer.ProduceAsync(
            topic,
            new Message<Null, string> { Value = payload },
            cancellationToken);

        Logger.LogInformation($"✅ Delivered to {result.TopicPartitionOffset}");
    }
}
