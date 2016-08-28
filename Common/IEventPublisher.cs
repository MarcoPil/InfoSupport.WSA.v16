using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Common
{
    /// <summary>
    /// Implementations for this interface should publish Domain Events on the 'Event Bus'
    /// </summary>
    public interface IEventPublisher
    {
        void Publish(DomainEvent domainEvent);
    }
}
