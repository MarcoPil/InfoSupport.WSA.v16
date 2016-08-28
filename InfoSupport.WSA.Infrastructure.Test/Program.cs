using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InfoSupport.WSA.Infrastructure.Test
{
    public class EventPublisherTest
    {
        [Fact]
        public void PublishEmitsEventOnExhange()
        {
            // Arrange (set the stage)
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "TestEventbus",
                                    type: RabbitMQ.Client.ExchangeType.Fanout);
            channel.QueueDeclare(queue: "TestQueue");

            channel.QueueBind(queue: "TestQueue",
                              exchange: "TestEventbus",
                              routingKey: "TestEventKey");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                throw new NotImplementedException();
            };

            channel.BasicConsume(consumer: consumer, queue: "TestQueue");

            // Arrange
            //IEventPublisher
        }

    }
}
