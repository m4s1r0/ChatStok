using ChatStock.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChatStock.Application.IoC.Services
{
    public static class ServicesIoC
    {
        public static void ApplicationServicesIoC(this IServiceCollection services)
        {
            services.AddScoped<IStockService, StockService>();
        }
    }
}
