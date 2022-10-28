using ChatStock.Application.Interfaces.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ChatStock.API.RabbitMQ
{
    public class RabbitMQProducer : IRabbitMQProducer
    {

        public RabbitMQProducer()
        {

        }

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("StockQuotes", exclusive: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: "StockQuotes", body: body);
        }
    }
}
