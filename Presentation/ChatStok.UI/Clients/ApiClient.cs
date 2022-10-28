using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChatStok.UI.Clients
{
    public class ApiClient
    {
        private HttpClient _client;


        public ApiClient()
        {
            _client = new HttpClient();

            _client.BaseAddress = new Uri("http://localhost:5115/");
        }

        public async Task GetStockQuote(string stockCode)
        {
            ResetClientRequestHeaders();
            var request = GetHttpRequestMessage($"api/stock/{stockCode}");
            var response = await _client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
            }
        }

        public void ResetClientRequestHeaders()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public HttpRequestMessage GetHttpRequestMessage(string relativePath)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, relativePath);
            request.Content = new StringContent("stock");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return request;
        }
    }
}
