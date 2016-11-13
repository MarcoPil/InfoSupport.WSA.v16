namespace InfoSupport.WSA.Infrastructure
{
    /// <summary>
    /// The BusOptions are used for configuring a connection to RabbitMQ.
    /// </summary>
    public class BusOptions
    {
        /// <summary>
        /// Default: "WSA.DefaultEventBus"
        /// </summary>
        public string ExchangeName { get; set; }
        /// <summary>
        /// The QueueName is only relevant for listeners. A null-value means that the listener will generate a random new queuename
        /// Default QueueName: null 
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// Default HostName: "localhost"
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// Default Port: 5672
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Default UserName: "guest"
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Default Password: "guest"
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Initializes with default BusOptions
        /// </summary>
        public BusOptions()
        {
            ExchangeName = "WSA.DefaultEventBus";
            QueueName = null;
            HostName = "localhost";
            Port = 5672;
            UserName = "guest";
            Password = "guest";
        }
    }
}