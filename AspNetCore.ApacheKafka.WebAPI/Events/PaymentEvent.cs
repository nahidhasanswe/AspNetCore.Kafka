

namespace AspNetCore.ApacheKafka.WebAPI.Events;

public record PaymentEvent(Guid PaymentId, Guid OrderId, decimal Amount);