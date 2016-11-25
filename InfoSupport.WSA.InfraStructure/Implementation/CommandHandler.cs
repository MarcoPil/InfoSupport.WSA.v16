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
    }
}