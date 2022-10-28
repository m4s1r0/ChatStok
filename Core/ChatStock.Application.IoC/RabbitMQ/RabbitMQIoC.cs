using ChatStock.API.RabbitMQ;
using ChatStock.Application.Interfaces.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace ChatStock.Application.IoC.RabbitMQ
{
    public static class RabbitMQIoC
    {
        public static void ApplicationRabbitMQIoC(this IServiceCollection services)
        {
            services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
        }
    }
}
