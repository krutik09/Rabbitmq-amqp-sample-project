using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var userName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "app2";
var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "app2pass";
var factor = new ConnectionFactory()
{
    HostName = rabbitMqHost,
    UserName = userName,
    Password = password,
    VirtualHost = "/"
};
var connection = factor.CreateConnection();
var channel = connection.CreateModel();
channel.ExchangeDeclare("poc_exchange", ExchangeType.Topic);
var queueName = channel.QueueDeclare(durable: true).QueueName;
channel.QueueBind(queueName, "poc_exchange", "poc.*.app2");
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    var routingKeyArray = ea.RoutingKey.Split('.');
    var sender = routingKeyArray[1];
    var receiver = routingKeyArray[2];
    Console.WriteLine($"[{sender}]:[{receiver}] :: {message}");
};
channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);
// Sending message to exchange
while (true)
{
    Console.WriteLine("write message to send or write exit");
    var message = Console.ReadLine();
    if (message == "exit"||message == null)
    {
        break;
    }
    var body = Encoding.UTF8.GetBytes(message);
    Console.WriteLine("send to which app?");
    string rKey = Console.ReadLine() ?? "admin";
    rKey = "poc.app2."+rKey;
    channel.BasicPublish(exchange: "poc_exchange", routingKey: rKey, basicProperties: null, body: body);
    Console.WriteLine($"[app2]: Sent {message} to {rKey}");
}
