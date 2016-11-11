namespace InfoSupport.WSA.Infrastructure
{
    public class BusOptions
    {
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Initializing with default BusOptions:
        ///   ExchangeName = "WSA.DefaultEventBus", 
        ///   QueueName = null, 
        ///   HostName = "localhost", 
        ///   Port = 5672, 
        ///   UserName = "guest", 
        ///   Password = "guest";
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