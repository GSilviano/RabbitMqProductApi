namespace RabbitMqProductApi.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        public Task SendProductMessage<T>(T message);
    }
}
