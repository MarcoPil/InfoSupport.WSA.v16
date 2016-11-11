using InfoSupport.WSA.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Infrastructure
{
    public class EventDispatcher : EventBusBase
    {
        public DispatcherModel DispatcherModel { get; }

        public EventDispatcher(BusOptions options = null) : base(options)
        {
            DispatcherModel = new DispatcherModel();
            CreateDispatcherModel();

            StartListening(BusOptions.QueueName);
        }

        private void CreateDispatcherModel()
        {
            var customType = this.GetType();
            var customMethods = customType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (var method in customMethods)
            {
                if (method.GetParameters().Length == 1)
                {
                    var paramType = method.GetParameters().First().ParameterType;
                    if (typeof(DomainEvent).IsAssignableFrom(paramType))
                    {
                        DispatcherModel.AddHandler(
                            paramType.FullName,
                            new DispatchHandler(this, method, paramType));
                    }
                }
            }
        }
        private void StartListening(string queueName)
        {
            if (queueName == null)
            {
                queueName = Channel.QueueDeclare().QueueName;
            }
            else
            {
                Channel.QueueDeclare(queue: queueName);
            }

            Channel.QueueBind(exchange: BusOptions.ExchangeName,
                              queue: queueName,
                              routingKey: "#");

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += EventReceived;

            Channel.BasicConsume(queue: queueName,
                                 noAck: true,
                                 consumer: consumer);
        }

        private void EventReceived(object sender, BasicDeliverEventArgs e)
        {
            var eventType = e.BasicProperties.Type;
            var message = Encoding.UTF8.GetString(e.Body);
            DispatcherModel.DispatchEvent(eventType, message);
        }
    }
}
