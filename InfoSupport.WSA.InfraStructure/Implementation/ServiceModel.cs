using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Infrastructure
{
    public class ServiceModel<T>
    {
        private Dictionary<string, CommandHandler<T>> _handlers;
        private List<string> _queueNames;

        public IEnumerable<string> Handlers => _handlers.Keys;
        public IEnumerable<string> QueueNames => _queueNames;

        public ServiceModel()
        {
            _handlers = new Dictionary<string, CommandHandler<T>>();
            _queueNames = new List<string>();
        }

        internal void Add(IEnumerable<KeyValuePair<string, CommandHandler<T>>> commandHandlerList)
        {
            foreach (var handler in commandHandlerList)
            {
                _handlers.Add(handler.Key, handler.Value);
            }
        }

        internal void AddIfNotEmpty(string queueName)
        {
            if (queueName != null)
            {
                _queueNames.Add(queueName);
            }
        }

        internal string DispatchCall(T instance, string eventName, string jsonMessage)
        {
            if (_handlers.ContainsKey(eventName))
            {
                return _handlers[eventName].DispatchCall(instance, jsonMessage);
            }
            return null; // TODO
        }
    }
}
