using AspNetCore.ApacheKafka.Core;
using AspNetCore.ApacheKafka.WebAPI.Consumers;
using AspNetCore.ApacheKafka.WebAPI.Events;

var builder = WebApplication.CreateBuilder(args);

var kafkaSettings = builder.Configuration.GetSection("Kafka").Get<KafkaSettings>();

builder.Services.AddKafkaProducer(builder.Configuration);

foreach (var consumer in kafkaSettings?.Consumers)
{
    if (consumer.Topic == "orders")
    {
        builder.Services.AddKafkaConsumer<OrderEvent, OrderEventHandler>(
                consumer.Topic,
                consumer.GroupId,
                consumer.AutoOffsetReset);
    }
    else if (consumer.Topic == "payments")
    {
        builder.Services.AddKafkaConsumer<PaymentEvent, PaymentEventHandler>(
                consumer.Topic,
                consumer.GroupId,
                consumer.AutoOffsetReset);
    }
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
