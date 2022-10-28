using ChatStok.UI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChatStok.UI.Controllers
{
    public class StockQuoteController : Controller
    {
        private readonly ILogger<StockQuoteController> _logger;
        readonly IHubContext<ChatHub> _chatHub;

        public StockQuoteController(ILogger<StockQuoteController> logger,
                                    IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
            _logger = logger;
        }

        [HttpGet]
        [Route("stock-quote/receive/{stockCode}/{stockQuote}")]
        public async Task<IActionResult> ReceiveStockQuote([FromRoute] string stockCode, [FromRoute] string stockQuote)
        {
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", "System", $"{stockCode} quote is ${stockQuote} per share");          
            
            return Ok();
        }
    }
}
