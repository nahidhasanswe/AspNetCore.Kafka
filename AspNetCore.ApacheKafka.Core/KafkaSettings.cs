namespace AspNetCore.ApacheKafka.Core;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public ProducerSettings Producer { get; set; } = new();
    public List<ConsumerSettings> Consumers { get; set; } = new();
}

public class ProducerSettings
{
    public string ClientId { get; set; } = "aspnet-producer";
}

public class ConsumerSettings
{
    public string GroupId { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string AutoOffsetReset { get; set; } = "Earliest";
}