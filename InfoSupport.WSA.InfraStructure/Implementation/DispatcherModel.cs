using System;
using System.Collections.Generic;

namespace InfoSupport.WSA.Infrastructure
{
    public class DispatcherModel
    {
        private Dictionary<string, DispatchHandler> _handlers;
        private List<string> _routingKeys;
        public IEnumerable<string> Handlers { get { return _handlers.Keys; } }
        public IEnumerable<string> RoutingKeys { get { return _routingKeys; } }

        public DispatcherModel()
        {
            _handlers = new Dictionary<string, DispatchHandler>();
            _routingKeys = new List<string>();
        }

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

        internal void AddRoutingKey(string routingKey)
        {
            _routingKeys.Add(routingKey);
        }
    }
}