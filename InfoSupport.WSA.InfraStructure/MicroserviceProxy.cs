using System;
using InfoSupport.WSA.Infrastructure;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

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

    public T Execute<T>(object command)
    {
        Channel.QueueDeclare(queue: BusOptions.QueueName,
                             durable: false, exclusive: false, autoDelete: false, arguments: null);
        var callbackQueueName = Channel.QueueDeclare().QueueName;
        EventWaitHandle responseReceived = new AutoResetEvent(false);
        T result = default(T);

        // set metadata
        var props = Channel.CreateBasicProperties();
        props.Type = command.GetType().FullName;
        props.ReplyTo = callbackQueueName;

        // set payload
        string message = JsonConvert.SerializeObject(command);
        var buffer = Encoding.UTF8.GetBytes(message);

        //Start Listening for callback
        var consumer = new EventingBasicConsumer(Channel);
        consumer.Received += (object sender, BasicDeliverEventArgs e) =>
        {
            var eventType = e.BasicProperties.Type;
            string body = Encoding.UTF8.GetString(e.Body);
            result = JsonConvert.DeserializeObject<T>(body);
            responseReceived.Set();
        };

        Channel.BasicConsume(queue: callbackQueueName,
                             noAck: true,
                             consumer: consumer);

        Channel.BasicPublish(exchange: "",
                             routingKey: BusOptions.QueueName,
                             basicProperties: props,
                             body: buffer);

        responseReceived.WaitOne();
        return result;
    }

}