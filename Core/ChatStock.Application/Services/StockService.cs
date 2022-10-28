using ChatStock.Application.IoC.HttpClients;
using ChatStock.Application.IoC.Services;
using ChatStock.Common.Models;

namespace ChatStock.Application.Services
{
    public class StockService : IStockService
    {
        private IStockClient _client;
        public StockService(IStockClient client)
        {
            _client = client;
        }
        public async Task<StockQuote> GetStock(string stockCode)
        {
            var result = await _client.GetStock(stockCode);
            return result;
        }
    }
}
