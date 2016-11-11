﻿using System;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace InfoSupport.WSA.Common
{
    /// <summary>
    /// Base class for all domain events.
    /// </summary>
    public abstract class DomainEvent
    {
        /// <summary>
        /// The Routing Key is used by the underlying protocol to route events to subscribers
        /// </summary>
        [JsonProperty]
        public string RoutingKey { get; private set; }

        /// <summary>
        /// The Timestamp has been set to the creation time of the domain event.
        /// </summary>
        [JsonProperty]
        public long Timestamp { get; private set; }

        /// <summary>
        /// Creates a domain event by setting the routing key and generating a timestamp.
        /// </summary>
        /// <param name="routingKey">The routing key should be of the format domain.eventname</param>
        public DomainEvent(string routingKey)
        {
            RoutingKey = routingKey;
            Timestamp = DateTime.Now.Ticks;
        }
    }

}