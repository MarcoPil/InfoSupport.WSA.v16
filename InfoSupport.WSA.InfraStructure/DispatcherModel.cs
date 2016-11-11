using System;
using System.Collections.Generic;

namespace InfoSupport.WSA.Infrastructure
{
    public class DispatcherModel
    {
        private Dictionary<string, DispatchHandler> _handlers = new Dictionary<string, DispatchHandler>();
        public IEnumerable<string> Handlers { get { return _handlers.Keys; } }

        internal void AddHandler(string eventName, DispatchHandler dispatchHandler)
        {
            _handlers.Add(eventName, dispatchHandler);
        }

        internal void DispatchEvent(string eventName, string jsonMessage)
        {
            if (_handlers.ContainsKey(eventName))
            {
                _handlers[eventName].DispatchEvent(jsonMessage);
            }
        }
    }
}