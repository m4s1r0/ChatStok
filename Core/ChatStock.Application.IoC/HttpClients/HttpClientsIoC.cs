using ChatStock.Application.HttpClients;
using Microsoft.Extensions.DependencyInjection;

namespace ChatStock.Application.IoC.HttpClients
{
    public static class HttpClientsIoC
    {
        public static void ApplicationHttpClientsIoC(this IServiceCollection services)
        {
            services.AddScoped<IStockClient, StockClient>();
        }
    }
}
