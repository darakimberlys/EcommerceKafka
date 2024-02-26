using System.Text.Json;
using Confluent.Kafka;
using EcommerceKafka.Data;
using EcommerceKafka.Services.Interfaces;

namespace EcommerceKafka.Services;

public class KafkaConsumerService : IKafkaConsumerService
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly string _topic;

    public KafkaConsumerService()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "ecommerce-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Latest
        };
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        _topic = "PEDIDOS";
    }

    public Order ConsumeMessages(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var cr = _consumer.Consume(cancellationToken);
                if (cr.Topic.Length == 0)
                {
                    return null;
                }
                var message = JsonSerializer.Deserialize<Order>(cr.Message.Value);
                Console.WriteLine($"Consumed message '{message}' at: '{cr.TopicPartitionOffset}'.");
                return message;

            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occurred: {e.Error.Reason}");
                throw new InvalidCastException();
            }
        }
        _consumer.Close();
        return null;

    }
}