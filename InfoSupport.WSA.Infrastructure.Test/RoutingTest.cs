﻿using InfoSupport.WSA.Infrastructure.Test.dummies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InfoSupport.WSA.Infrastructure.Test
{
    public class RoutingTest
    {
        [Fact]
        public void RoutingKeysAreRegistered()
        {
            using (var result = new RoutedDispatherMock())
            {
                Assert.Contains("WSA.Routed.*", result.DispatcherModel.RoutingKeys);
                Assert.Contains("Multiple.#", result.DispatcherModel.RoutingKeys);
            }
        }

        [Fact]
        public void RoutingKeyWorksWithAllowingKey()
        {
            BusOptions options = new BusOptions { ExchangeName = "TestExchange_RoutingTest" };
            using (var publisher = new EventPublisher(options))
            using (var target = new RoutedDispatherMock(options))
            {
                publisher.Publish(new RoutedEvent());

                Thread.Sleep(100);

                Assert.False(target.TestEventHandlerHasBeenCalled, "TestEventHandler should NOT have been called");
                Assert.True(target.RoutedEventHandlerHasBeenCalled, "RoutedEventHandler SHOULD have been called");
            }
        }


        [Fact]
        public void RoutingKeyWorksWithNonAllowingKey()
        {
            BusOptions options = new BusOptions { ExchangeName = "TestExchange_RoutingTest" };
            using (var publisher = new EventPublisher(options))
            using (var target = new RoutedDispatherMock(options))
            {
                publisher.Publish(new TestEvent());

                Thread.Sleep(100);

                Assert.False(target.TestEventHandlerHasBeenCalled, "TestEventHandler should NOT have been called");
                Assert.False(target.RoutedEventHandlerHasBeenCalled, "RoutedEventHandler should NOT have been called");
            }
        }
    }
}