using EcommerceKafka.Data;

namespace EcommerceKafka.Services.Interfaces;

public interface IKafkaProducerService
{
    Task SendMessageAsync(Order message);
}