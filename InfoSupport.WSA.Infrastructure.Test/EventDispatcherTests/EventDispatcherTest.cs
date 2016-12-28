﻿using InfoSupport.WSA.Infrastructure;
using InfoSupport.WSA.Infrastructure.Test;
using InfoSupport.WSA.Infrastructure.Test.dummies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class EventDispatcherTest
{
    [Fact]
    public void EventDispatcherFindsDispatchMethods()
    {
        using (var result = new EventDispatcherMock())
        {
            Assert.Equal(2, result.DispatcherModel.Handlers.Count());
        }
    }

    [Fact]
    public void EventDispatcherDispatchesEvents()
    {
        using (var publisher = new EventPublisher())
        using (var target = new EventDispatcherMock())
        {
            target.Open();

            publisher.Publish(new TestEvent());

            Thread.Sleep(100);

            Assert.True(target.TestEventHandlerHasBeenCalled);
        }
    }

    [Fact]
    public void EventDispatcherDispatchesToCorrectEventHanlder()
    {
        using (var publisher = new EventPublisher())
        using (var target = new EventDispatcherMock())
        {
            target.Open();

            publisher.Publish(new AnotherEvent());

            Thread.Sleep(100);

            Assert.False(target.TestEventHandlerHasBeenCalled);
            Assert.True(target.AnotherEventHandlerHasBeenCalled);
        }
    }

    [Fact]
    public void EventDispatcherDispatchesEventWithCorrectData()
    {
        using (var publisher = new EventPublisher())
        using (var target = new EventDispatcherMock())
        {
            target.Open();

            publisher.Publish(new AnotherEvent() { SomeValue = 7 });

            Thread.Sleep(100);

            Assert.Equal(7, target.SomeValue);
        }
    }

    [Fact]
    public void EventDispatcherDispatchesEventWithCorrectTimestamp()
    {
        using (var publisher = new EventPublisher())
        using (var target = new EventDispatcherMock())
        {
            target.Open();

            var evt = new TestEvent();
            publisher.Publish(evt);

            Thread.Sleep(100);

            Assert.Equal(evt.Timestamp, target.SomeTimestamp);
        }
    }
}