using EcommerceKafka.Data;

namespace EcommerceKafka.Services.Interfaces;

public interface IKafkaConsumerService
{
    Order ConsumeMessages(CancellationToken cancellationToken);
}