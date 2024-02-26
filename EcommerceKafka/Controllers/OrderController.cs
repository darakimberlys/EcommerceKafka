using EcommerceKafka.Data;
using EcommerceKafka.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceKafka.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IKafkaProducerService _kafkaProducerService;
    private readonly IKafkaConsumerService _kafkaConsumerService;
    
    public OrderController(IKafkaProducerService kafkaProducerService, IKafkaConsumerService kafkaConsumerService)
    {
        _kafkaProducerService = kafkaProducerService;
        _kafkaConsumerService = kafkaConsumerService;
    }
    
    [HttpPost("order")]
    public async Task<IActionResult> PostOrder([FromBody] Order value)
    {
        if (string.IsNullOrWhiteSpace(value.Product)) return BadRequest();
        
        await _kafkaProducerService.SendMessageAsync(value);
        return Ok();
    }
    
    [HttpGet("order")]
    public Task<IActionResult> GetOrders()
    {
        var result =  _kafkaConsumerService.ConsumeMessages(new CancellationToken());
        return Task.FromResult<IActionResult>(Ok(result));
    }
}