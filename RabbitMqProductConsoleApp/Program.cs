using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
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
//Set Event object which listen message from chanel which is sent by producer
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Product message received: {message}");
    return Task.CompletedTask;
};
//read the message
await channel.BasicConsumeAsync(queue: "product", autoAck: true, consumer: consumer);
Console.ReadKey();