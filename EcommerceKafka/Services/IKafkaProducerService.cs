using EcommerceKafka.Data;

namespace EcommerceKafka.Services;

public interface IKafkaProducerService
{
    Task SendMessageAsync(Order message);
}