using AspNetCore.ApacheKafka.Core;
using AspNetCore.ApacheKafka.WebAPI.Events;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.ApacheKafka.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IKafkaProducer _producer;

        public TestController(IKafkaProducer producer)
        {
            _producer = producer;
        }

        [HttpPost("order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderEvent order)
        {
            await _producer.ProduceAsync("orders", order);
            return Ok(new { Status = "Sent", order.OrderId });
        }

        [HttpPost("payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentEvent payment)
        {
            await _producer.ProduceAsync("payments", payment);
            return Ok(new { Status = "Sent", payment.PaymentId });
        }
    }
}
