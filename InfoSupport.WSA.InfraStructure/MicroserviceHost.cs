using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InfoSupport.WSA.Infrastructure
{
    public class MicroserviceHost<T> : EventBusBase
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
            StartListening();
        }

        private void AddQueueNamesToServiceModel()
        {
            // gather queue names from [MicroService(queueName)]-attributes
            var queueNames = from Interface in AllInterfacesOf<T>()
                             let queueName = Interface.GetTypeInfo().GetCustomAttributes<MicroserviceAttribute>().FirstOrDefault()?.QueueName
                             where queueName != null
                             select queueName;
            ServiceModel.Add(queueNames);

            // gather queue name from BusOptions
            if (BusOptions.QueueName != null)
            {
                ServiceModel.Add(new string[] { BusOptions.QueueName } );
            }

        }

        private void AddCommandHandlersToServiceModel()
        {
            var commandHandlerList =
                from Interface in AllInterfacesOf<T>()
                where HasMicroserviceAttribute(Interface)
                    from interfaceMethod in Interface.GetMethods()
                    where interfaceMethod.GetParameters().Length == 1
                    let commandType = GetCommandType(interfaceMethod)
                    select new KeyValuePair<string, CommandHandler<T>>
                    (
                        commandType.FullName,
                        new CommandHandler<T>(interfaceMethod, commandType)
                    );

            ServiceModel.Add(commandHandlerList);  
        }
        #region PopulateDispatcherModel() - helper methods
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
        #endregion PopulateDispatcherModel() - helper methods

        private void StartListening()
        {
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
            var instance = _instance ?? new T();    // InstanceContext=SingleCall
            var eventType = e.BasicProperties.Type;
            var message = Encoding.UTF8.GetString(e.Body);
            ServiceModel.DispatchCall(instance, eventType, message);
        }
    }
}