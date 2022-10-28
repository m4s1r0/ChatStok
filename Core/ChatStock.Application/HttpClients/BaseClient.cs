using System.Net.Http.Headers;

namespace ChatStock.Application.HttpClients
{
    public abstract class BaseClient
    {
        public void ResetClientRequestHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public HttpRequestMessage GetHttpRequestMessage(string relativePath, string content, HttpMethod httpMethod)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, relativePath);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return request;
        }

        public HttpRequestMessage GetHttpRequestMessage(string relativePath, HttpMethod httpMethod)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, relativePath);

            return request;
        }
    }
}
