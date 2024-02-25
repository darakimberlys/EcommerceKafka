using System.Text.Json;
using Confluent.Kafka;
using EcommerceKafka.Data;

namespace EcommerceKafka.Services;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducerService()
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _topic = "PEDIDO";
    }

    public async Task SendMessageAsync(Order message) 
    {
        if (string.IsNullOrWhiteSpace(message.Product) ^ (message.Quantity == 0))
        {
            throw new InvalidDataException();
        }

        try
        {
            var dr = await _producer.ProduceAsync(_topic,
                new Message<Null, string> { Value = JsonSerializer.Serialize(message) });
            Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
}