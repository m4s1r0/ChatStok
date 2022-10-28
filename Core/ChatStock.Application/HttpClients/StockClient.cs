using ChatStock.Application.IoC.HttpClients;
using ChatStock.Application.SeedWork;
using ChatStock.Common.Helpers;
using ChatStock.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChatStock.Application.HttpClients
{
    public class StockClient : IStockClient
    {
        private ILogger<StockClient> _logger;
        private HttpClient _client;
        private readonly AppSettings _appSettings;
        private const int VALUE_ROW_INDEX = 1;
        private const int VALUE_COL_INDEX = 6;
        private const int CODE_COL_INDEX = 6;


        public StockClient(ILogger<StockClient> logger,
                           HttpClient client,
                           IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _client = client;
        }

        public async Task<StockQuote> GetStock(string stockCode)
        {
            try
            {
                var builder = new UriBuilder("https://stooq.com/q/l/");
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["s"] = stockCode;
                query["f"] = "sd2t2ohlcv";
                query["h"] = String.Empty;
                query["e"] = "csv";
                builder.Query = query.ToString();
                string url = builder.ToString();

                var action = String.Format("?s={0}&f=sd2t2ohlcv&h&e=csv", stockCode);
                //this.ResetClientRequestHeaders(_client);
                var request = GetHttpRequestMessage(HttpMethod.Get, url);
                var response = await _client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                CSVHelper csv = new CSVHelper(result, ",");

                var sq = new StockQuote()
                {
                    Code = stockCode,
                    Quote = csv[VALUE_ROW_INDEX][VALUE_COL_INDEX]
                };

                return sq/*.ToJson()*/;
            }
            catch (Exception)
            {

                throw;
            }

            
        }

        public HttpRequestMessage GetHttpRequestMessage(HttpMethod httpMethod, string requestUri)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, requestUri);

            return request;
        }

    }
}
