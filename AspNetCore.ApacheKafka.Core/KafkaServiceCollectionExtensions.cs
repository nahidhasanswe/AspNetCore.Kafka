using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCore.ApacheKafka.Core;

public static class KafkaServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaProducer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        services.AddSingleton<IKafkaProducer, KafkaProducer>();
        return services;
    }

     public static IServiceCollection AddKafkaProducer(this IServiceCollection services, KafkaSettings settings)
    {
        services.AddSingleton(Options.Create(settings));
        services.AddSingleton<IKafkaProducer, KafkaProducer>();
        return services;
    }

    public static IServiceCollection AddKafkaConsumer<TEvent, THandler>(
        this IServiceCollection services,
        string topic,
        string groupId,
        string autoOffsetReset = "Earliest")
        where THandler : class, IKafkaConsumerHandler<TEvent>
    {
        services.AddSingleton<IKafkaConsumerHandler<TEvent>, THandler>();
        services.AddHostedService(sp =>
            new KafkaConsumerService<TEvent>(
                sp.GetRequiredService<IOptions<KafkaSettings>>(),
                sp.GetRequiredService<IKafkaConsumerHandler<TEvent>>(),
                sp.GetRequiredService<ILogger<KafkaConsumerService<TEvent>>>(),
                topic,
                groupId,
                autoOffsetReset));

        return services;
    }
}
