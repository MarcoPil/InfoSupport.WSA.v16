using InfoSupport.WSA.Infrastructure.Test.dummies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InfoSupport.WSA.Infrastructure.Test
{
    public class MicroserviceHostAndProxyTest
    {
        [Fact]
        public void HandlerNotInInterfaceShouldNotBeTriggered()
        {
            var options = new BusOptions() { QueueName = "TestQueue01" };
            var serviceMock = new HalfServiceMock();
            using (var host = new MicroserviceHost<HalfServiceMock>(serviceMock, options))
            using (var proxy = new MicroserviceProxy(options))
            {
                host.Open();

                var command = new SomeCommand() { SomeValue = "teststring" };
                proxy.Execute(command);

                serviceMock.receivedFlag.WaitOne(500);
                Assert.False(serviceMock.SomeCommandHandlerHasBeenCalled);
            }
        }

        [Fact]
        public void HandlerNotInInterfaceShouldNotBeTriggered2()
        {
            var options = new BusOptions() { QueueName = "TestQueue02" };
            var serviceMock = new HalfServiceMock();
            using (var host = new MicroserviceHost<HalfServiceMock>(serviceMock, options))
            using (var proxy = new MicroserviceProxy(options))
            {
                host.Open();

                var command = new TestCommand();
                proxy.Execute(command);

                serviceMock.receivedFlag.WaitOne(500);
                Assert.True(serviceMock.TestCommandHandlerHasBeenCalled);
            }
        }
    }
}
