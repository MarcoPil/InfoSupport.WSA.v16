using Newtonsoft.Json;
using System;
using System.Reflection;

namespace InfoSupport.WSA.Infrastructure
{
    internal class CommandHandler<T>
    {
        private MethodInfo _method;
        private Type _commandType;

        public CommandHandler(MethodInfo method, Type commandType)
        {
            _method = method;
            _commandType = commandType;
        }

        internal void DispatchCall(T instance, string jsonMessage)
        {
            var command = JsonConvert.DeserializeObject(jsonMessage, _commandType);

            _method.Invoke(instance, new object[] { command });
        }
    }
}