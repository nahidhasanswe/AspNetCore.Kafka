namespace AspNetCore.ApacheKafka.WebAPI.Events;
public record OrderEvent(Guid OrderId, decimal Amount, DateTime CreatedAt);