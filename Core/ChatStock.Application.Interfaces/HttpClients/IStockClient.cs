using ChatStock.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStock.Application.IoC.HttpClients
{
    public interface IStockClient
    {
        public Task<StockQuote> GetStock(string stockCode);
    }
}
