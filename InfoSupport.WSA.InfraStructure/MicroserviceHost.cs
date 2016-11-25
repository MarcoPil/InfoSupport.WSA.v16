using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfoSupport.WSA.Infrastructure
{
    public class MicroserviceHost<T> : IDisposable
    {
        public ServiceModel<T> ServiceModel { get; private set; }

        public MicroserviceHost()
        {
            ServiceModel = new ServiceModel<T>();
            AddCommandHandlersToServiceModel();
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

        public void Dispose()
        {
        }
    }
}