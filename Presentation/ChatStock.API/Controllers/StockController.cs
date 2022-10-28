using ChatStock.API.RabbitMQ;
using ChatStock.Application.Interfaces.RabbitMQ;
using ChatStock.Application.IoC.HttpClients;
using ChatStock.Application.IoC.Services;
using ChatStock.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ChatStock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private ILogger<StockController> _logger;
        private IOptions<AppSettings> _settings;
        private IStockService _service;
        private readonly IRabbitMQProducer _messagePublisher;

        public StockController(ILogger<StockController> logger,
                               IStockService service,
                               IOptions<AppSettings> settings,
                               IRabbitMQProducer messagePublisher)
        {
            _logger = logger;
            _service = service;
            _settings = settings;
            _messagePublisher = messagePublisher;
        }

        [HttpGet("{stockCode}")]
        public async Task<IActionResult> GetStock([FromRoute] string stockCode = "")
        {
            var result = await _service.GetStock(stockCode);

            _messagePublisher.SendMessage(result);

            return Ok(result);
        }
    }
}
