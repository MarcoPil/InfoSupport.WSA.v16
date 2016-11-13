﻿using System;
using InfoSupport.WSA.Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace InfoSupport.WSA.Infrastructure
{

    public class EventPublisher : EventBusBase, IEventPublisher
    {
        /// <summary>
        /// Each EventPublisher creates its own connection to rabbitMQ.
        /// </summary>
        /// <param name="options">the configuration of the RabbitMQ connection. If none are passed, the default BusOptions are being used.</param>
        public EventPublisher(BusOptions options = null) : base(options) { }

        /// <summary>
        /// Publishes a domain event on the event bus (configured by the BusOptions).
        /// Make sure that the appropriate Routing Key has been set in the DomainEvent.
        /// </summary>
        /// <param name="domainEvent">The domain event to be published. </param>
        public void Publish(DomainEvent domainEvent)
        {
                // set metadata
                var props = Channel.CreateBasicProperties();
                props.Timestamp = new AmqpTimestamp(domainEvent.Timestamp);
                props.Type = domainEvent.GetType().FullName;
                // set payload
                string message = JsonConvert.SerializeObject(domainEvent);
                var buffer = Encoding.UTF8.GetBytes(message);
                // publish event
                Channel.BasicPublish(exchange: BusOptions.ExchangeName,
                                     routingKey: domainEvent.RoutingKey,
                                     basicProperties: props,
                                     body: buffer);
        }
    }
}