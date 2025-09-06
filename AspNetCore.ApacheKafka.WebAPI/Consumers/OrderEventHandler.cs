using System;
using AspNetCore.ApacheKafka.Core;
using AspNetCore.ApacheKafka.WebAPI.Events;

namespace AspNetCore.ApacheKafka.WebAPI.Consumers;

public class OrderEventHandler : IKafkaConsumerHandler<OrderEvent>
{
    public Task HandleAsync(OrderEvent message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"📦 Order Received: {message.OrderId}, Amount: {message.Amount}");
        return Task.CompletedTask;
    }
}