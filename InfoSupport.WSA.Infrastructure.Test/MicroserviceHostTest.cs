using InfoSupport.WSA.Infrastructure.Test.dummies;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InfoSupport.WSA.Infrastructure.Test
{
    public class MicroserviceHostTest
    {
        [Fact]
        public void MicroserviceHostFindsHandlers()
        {
            using (var host = new MicroserviceHost<SomeMicroserviceMock>())
            {
                Assert.Equal(2, host.ServiceModel.Handlers.Count());
                Assert.True(host.ServiceModel.Handlers.Contains("InfoSupport.WSA.Infrastructure.Test.dummies.TestCommand"), "TestCommand is not recognized");
                Assert.True(host.ServiceModel.Handlers.Contains("InfoSupport.WSA.Infrastructure.Test.dummies.SomeCommand"), "SomeCommand is not recognized");
            }
        }

        [Fact]
        public void MicroserviceHostFindsHandlersInInstance()
        {
            var serviceMock = new SomeMicroserviceMock();
            using (var host = new MicroserviceHost<SomeMicroserviceMock>(serviceMock))
            {
                Assert.Equal(2, host.ServiceModel.Handlers.Count());
                Assert.True(host.ServiceModel.Handlers.Contains("InfoSupport.WSA.Infrastructure.Test.dummies.TestCommand"), "TestCommand is not recognized");
                Assert.True(host.ServiceModel.Handlers.Contains("InfoSupport.WSA.Infrastructure.Test.dummies.SomeCommand"), "SomeCommand is not recognized");
            }
        }


        [Fact]
        public void MicroserviceHostFindsQueueName()
        {
            var serviceMock = new SomeMicroserviceMock();
            using (var host = new MicroserviceHost<SomeMicroserviceMock>(serviceMock))
            {
                Assert.Equal(1, host.ServiceModel.QueueNames.Count());
                Assert.True(host.ServiceModel.QueueNames.Contains("microserviceQueue"));
            }
        }


        [Fact]
        public void MicroserviceHostReceivesCommands()
        {
            var serviceMock = new SomeMicroserviceMock();
            using (var host = new MicroserviceHost<SomeMicroserviceMock>(serviceMock))
            {
                SendMessage();
                Thread.Sleep(500);

                Assert.True(serviceMock.SomeCommandHandlerHasBeenCalled);
            }
        }

        private void SendMessage()
        {
            var BusOptions = new BusOptions() { QueueName = "microserviceQueue" };

            var factory = new ConnectionFactory()
            {
                HostName = BusOptions.HostName,
                Port = BusOptions.Port,
                UserName = BusOptions.UserName,
                Password = BusOptions.Password,
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: BusOptions.QueueName,
                                     durable: false, exclusive: false, autoDelete: false, arguments: null);

                var command = new SomeCommand() { SomeValue = "teststring" };
                // set metadata
                var props = channel.CreateBasicProperties();
                props.Type = command.GetType().FullName;
                // set payload
                string message = JsonConvert.SerializeObject(command);
                var buffer = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: BusOptions.QueueName,
                                     basicProperties: props,
                                     body: buffer);
            }
        }
    }
}
