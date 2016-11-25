using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Infrastructure
{
    public class ServiceModel<T>
    {
        private Dictionary<string, CommandHandler<T>> _handlers;

        public IEnumerable<string> Handlers => _handlers.Keys;

        public ServiceModel()
        {
            _handlers = new Dictionary<string, CommandHandler<T>>();
        }

        internal void Add(IEnumerable<KeyValuePair<string, CommandHandler<T>>> commandHandlerList)
        {
            foreach (var handler in commandHandlerList)
            {
                _handlers.Add(handler.Key, handler.Value);
            }
        }
    }
}
