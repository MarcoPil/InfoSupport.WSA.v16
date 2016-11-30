using InfoSupport.WSA.Infrastructure.Test.dummies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InfoSupport.WSA.Infrastructure.Test
{
    public class MicroserviceProxyTest
    {
        [Fact]
        public void ProxySendsCommands()
        {
            var options = new BusOptions() { QueueName = "OtherQueue" };
            var serviceMock = new OtherMicroserviceMock();
            using (var host = new MicroserviceHost<OtherMicroserviceMock>(serviceMock, options))
            using (var proxy = new MicroserviceProxy(options))
            {
                var command = new SomeCommand() { SomeValue = "teststring" };
                proxy.Execute(command);

                Thread.Sleep(500);

                Assert.True(serviceMock.SomeCommandHandlerHasBeenCalled);
            }
        }

        [Fact]
        public void ProxySendsCommandsAndTheyAreReceivedCorrectly()
        {
            var options = new BusOptions() { QueueName = "OtherQueue" };
            var serviceMock = new OtherMicroserviceMock();
            using (var host = new MicroserviceHost<OtherMicroserviceMock>(serviceMock, options))
            using (var proxy = new MicroserviceProxy(options))
            {
                var command = new SomeCommand() { SomeValue = "teststring" };
                proxy.Execute(command);

                serviceMock.ReceivedFlag.WaitOne(500);

                Assert.True(serviceMock.SomeCommandHandlerHasBeenCalled);
                Assert.Equal("teststring", serviceMock.SomeCommandHandlerReceivedSomeCommand.SomeValue);
            }
        }
    }
}
