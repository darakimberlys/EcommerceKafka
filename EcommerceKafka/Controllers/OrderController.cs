using System.Text.Json;
using EcommerceKafka.Data;
using EcommerceKafka.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceKafka.Controllers;

[ApiController]
[Route("[controller]")]
public class SendOrderController : ControllerBase
{
    private readonly IKafkaProducerService _kafkaProducerService;
    
    public SendOrderController(IKafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
    }
    
    [HttpPost("order")]
    public IActionResult Post([FromBody] Order value)
    {
        if (string.IsNullOrWhiteSpace(value.Product)) return BadRequest();
        
        _kafkaProducerService.SendMessageAsync(value);
        return Ok();
    }
}