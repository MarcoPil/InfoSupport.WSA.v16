using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InfoSupport.WSA.Infrastructure
{
    public class MicroserviceHost<T> : ServiceBusBase
        where T: class, new()
    {
        private T _instance;

        public ServiceModel<T> ServiceModel { get; private set; }

        public MicroserviceHost() : this(null, null) { }
        public MicroserviceHost(T singletonInstance) : this(singletonInstance, null) { }
        public MicroserviceHost(BusOptions busOptions) : this(null, busOptions) { }
        public MicroserviceHost(T singletonInstance, BusOptions busOptions) : base(busOptions)
        {
            _instance = singletonInstance;
            ServiceModel = new ServiceModel<T>();
            AddQueueNamesToServiceModel();
            AddCommandHandlersToServiceModel();
        }

        private void AddQueueNamesToServiceModel()
        {
            // gather queue names from [MicroService(queueName)]-attributes
            foreach (var Interface in AllInterfacesOf<T>())
            {
                var attrs = Interface.GetTypeInfo().GetCustomAttributes<MicroserviceAttribute>();
                if (attrs.Any())
                {
                    ServiceModel.AddIfNotEmpty(attrs.First().QueueName);
                }
            }
            // gather queue name from BusOptions
            ServiceModel.AddIfNotEmpty(BusOptions.QueueName);
        }

        private void AddCommandHandlersToServiceModel()
        {
            var microserviceInterfaces = AllInterfacesOf<T>().Where(HasMicroserviceAttribute);

            if (!microserviceInterfaces.Any())
            {
                throw new MicroserviceConfigurationException("No [MicroService] interfaces have been found.");
            }
            else
            {
                var commandHandlerList =
                    from Interface in microserviceInterfaces
                    from interfaceMethod in Interface.GetMethods()
                    where interfaceMethod.GetParameters().Length == 1
                    let commandType = GetCommandType(interfaceMethod)
                    select new KeyValuePair<string, CommandHandler<T>>
                (
                    commandType.FullName,
                    new CommandHandler<T>(interfaceMethod, commandType)
                );

                if (!commandHandlerList.Any())
                {
                    throw new MicroserviceConfigurationException("No Handlers can be found in the Microservice interface.");
                }
                else
                {
                    ServiceModel.Add(commandHandlerList);
                }
            }

        }
        #region PopulateServiceModel() - helper methods
        private static Type[] AllInterfacesOf<U>()
        {
            return typeof(U).GetInterfaces();
        }
        private static bool HasMicroserviceAttribute(Type interfaceType)
        {
            return interfaceType.GetTypeInfo().GetCustomAttributes<MicroserviceAttribute>().Any();
        }
        private static Type GetCommandType(MethodInfo method)
        {
            return method.GetParameters().First().ParameterType;
        }
        #endregion PopulateServiceModel() - helper methods

        public override void Open()
        {
            // Open a RabbitMQ connection
            base.Open();

            foreach (var queueName in ServiceModel.QueueNames)
            {
                Channel.QueueDeclare(queue:queueName, 
                                     durable:false, exclusive:false, autoDelete:false, arguments:null);

                var consumer = new EventingBasicConsumer(Channel);
                consumer.Received += EventReceived;

                Channel.BasicConsume(queue: queueName,
                                     noAck: true,
                                     consumer: consumer);
            }
        }

        private void EventReceived(object sender, BasicDeliverEventArgs e)
        {
            var instance = _instance ?? new T();    // InstanceContext=Singleton ?? SingleCall
            var eventType = e.BasicProperties.Type;
            var message = Encoding.UTF8.GetString(e.Body);

            var responseMessage = ServiceModel.DispatchCall(instance, eventType, message);

            if (e.BasicProperties.ReplyTo != null)
            {
                // set metadata
                var props = Channel.CreateBasicProperties();
                // set payload
                var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                // publish event
                Channel.BasicPublish(exchange: "",
                                     routingKey: e.BasicProperties.ReplyTo,
                                     basicProperties: props,
                                     body: responseBuffer);
            }
        }
    }
}