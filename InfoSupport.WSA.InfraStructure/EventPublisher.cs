using System;
using InfoSupport.WSA.Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace InfoSupport.WSA.Infrastructure
{

    public class EventPublisher : EventBusBase, IEventPublisher
    {
        public EventPublisher(BusOptions options = null) : base(options) {}

        public void Publish(DomainEvent domainEvent)
        {
                var props = Channel.CreateBasicProperties();
                props.Timestamp = new AmqpTimestamp(domainEvent.Timestamp);
                props.Type = domainEvent.GetType().FullName;

                string message = JsonConvert.SerializeObject(domainEvent);
                var buffer = Encoding.UTF8.GetBytes(message);

                Channel.BasicPublish(exchange: BusOptions.ExchangeName,
                                     routingKey: domainEvent.RoutingKey,
                                     basicProperties: props,
                                     body: buffer);
        }
    }
}