namespace AspNetCore.ApacheKafka.Core;

public interface IKafkaConsumerHandler<T>
{
    Task HandleAsync(T message, CancellationToken cancellationToken);
}
