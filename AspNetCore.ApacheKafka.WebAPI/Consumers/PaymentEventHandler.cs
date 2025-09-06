using System;
using AspNetCore.ApacheKafka.Core;
using AspNetCore.ApacheKafka.WebAPI.Events;

namespace AspNetCore.ApacheKafka.WebAPI.Consumers;


public class PaymentEventHandler : IKafkaConsumerHandler<PaymentEvent>
{
    public Task HandleAsync(PaymentEvent message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"💰 Payment Received: {message.PaymentId}, Order: {message.OrderId}");
        return Task.CompletedTask;
    }
}