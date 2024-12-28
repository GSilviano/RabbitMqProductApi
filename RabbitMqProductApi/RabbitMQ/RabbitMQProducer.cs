using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqProductApi.RabbitMQ
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public async Task SendProductMessage<T>(T message)
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = await factory.CreateConnectionAsync();
            //Here we create channel with session and model
            using
            var channel = await connection.CreateChannelAsync();
            //declare the queue after mentioning name and a few property related to that
            await channel.QueueDeclareAsync("product", exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            await channel.BasicPublishAsync(exchange: "", routingKey: "product", body: body);
        }
    }
}
