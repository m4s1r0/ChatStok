using ChatStock.Common.Helpers;
using ChatStock.Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

static class Program 
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        //Create the RabbitMQ connection using connection factory details as i mentioned above
        var connection = factory.CreateConnection();
        //Here we create channel with session and model
        using
        var channel = connection.CreateModel();
        //declare the queue after mentioning name and a few property related to that
        channel.QueueDeclare("StockQuotes", exclusive: false);
        //Set Event object which listen message from chanel which is sent by producer
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, eventArgs) => {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var sq = message.FromJson<StockQuote>();

            await ProcessMessage(sq.Code, sq.Quote);
            Console.WriteLine($"Stock quote received: {message}");
        };
        //read the message
        channel.BasicConsume(queue: "StockQuotes", autoAck: true, consumer: consumer);
        Console.ReadKey();
    }

    private static async Task ProcessMessage(string stockCode, string stockQuote)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", "ASP.NET");

        var stringTask = client.GetStringAsync($"http://localhost:6116/stock-quote/receive/{stockCode}/{stockQuote}");

        var msg = await stringTask;
        Console.Write(msg);
    }
}