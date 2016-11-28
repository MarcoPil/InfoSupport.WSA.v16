using System;
using InfoSupport.WSA.Infrastructure;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;

public class MicroserviceProxy : EventBusBase
{
    public MicroserviceProxy(BusOptions options = null) : base(options)
    {
    }

    public void Execute(object command)
    {
        Channel.QueueDeclare(queue: BusOptions.QueueName,
                             durable: false, exclusive: false, autoDelete: false, arguments: null);

        // set metadata
        var props = Channel.CreateBasicProperties();
        props.Type = command.GetType().FullName;
        // set payload
        string message = JsonConvert.SerializeObject(command);
        var buffer = Encoding.UTF8.GetBytes(message);

        Channel.BasicPublish(exchange: "",
                             routingKey: BusOptions.QueueName,
                             basicProperties: props,
                             body: buffer);

    }
}