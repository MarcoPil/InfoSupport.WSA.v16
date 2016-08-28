using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Common
{
    /// <summary>
    /// Implementations for this interface should subscribe to Domain Events on the 'Event Bus'
    /// For each Domain Event it receives, it must execute the callback handler that has been registered for that type, 
    ///     or ignore the event if no callback has been registered for that type.
    ///     
    /// For each distinct DomainEvent Type, at most one callback/handler can be registered.
    /// When Events no longer should be dispatched, the EventDispatcher should be disposed.
    /// </summary>
    public interface IEventDispatcher : IDisposable
    {
        void AddHandler<T>(Action<T> callback) where T : DomainEvent;
        void RemoveHandler<T>() where T : DomainEvent;
    }
}
