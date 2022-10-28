namespace ChatStock.API.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        void SendMessage<T>(T message);
    }
}
