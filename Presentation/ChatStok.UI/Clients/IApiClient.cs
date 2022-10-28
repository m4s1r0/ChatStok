using System.Threading.Tasks;

namespace ChatStok.UI.Clients
{
    public interface IApiClient
    {
        public Task GetStockQuote(string stockCode);
    }
}
