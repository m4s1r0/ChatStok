using ChatStok.UI.Clients;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatStok.UI.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            if (message.StartsWith("stock:", System.StringComparison.OrdinalIgnoreCase))
            {
                var stockCode = message.Split(":")[1];
                var client = new ApiClient();

                try
                {
                    await client.GetStockQuote(stockCode);
                    //await Clients.Caller.SendAsync("ReceiveMessage", "System", $"You requested a stock quote for {stockCode}, please wait");
                }
                catch (System.Exception exc)
                {
                    await Clients.Caller.SendAsync("ReceiveMessage", "System", $"An error occurred, please try again. {exc.Message}");
                }

                return;
            }
            
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
