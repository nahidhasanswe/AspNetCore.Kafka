using System;

namespace AspNetCore.ApacheKafka.Core;

public interface IKafkaProducer
{
    Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken = default);
}
