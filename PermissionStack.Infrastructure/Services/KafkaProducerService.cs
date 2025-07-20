using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;

public class KafkaProducerService : IPermissionEventPublisher
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducerService(IConfiguration config)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config["Kafka:BootstrapServers"]
        };

        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        _topic = config["Kafka:Topic"] ?? "permission-operations"; // Valor por defecto
    }

    public async Task PublishAsync(PermissionOperationEventDto dto)
    {
        var message = JsonConvert.SerializeObject(dto);

        await _producer.ProduceAsync(_topic, new Message<Null, string>
        {
            Value = message
        });
    }
}


