﻿using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Infrastructure
{
    public abstract class EventBusBase : IDisposable
    {
        private IConnection _connection;
        protected IModel Channel;

        public BusOptions BusOptions { get; set; }

        public EventBusBase(BusOptions options = null)
        {
            BusOptions = options ?? new BusOptions();

            var factory = new ConnectionFactory()
            {
                HostName = BusOptions.HostName,
                Port =     BusOptions.Port,
                UserName = BusOptions.UserName,
                Password = BusOptions.Password,
            };
            _connection = factory.CreateConnection();
            Channel = _connection.CreateModel();

            Channel.ExchangeDeclare(exchange: BusOptions.ExchangeName,
                                    type: ExchangeType.Topic,
                                    durable: false, autoDelete: false, arguments: null);
        }

        public void Dispose()
        {
            Channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
