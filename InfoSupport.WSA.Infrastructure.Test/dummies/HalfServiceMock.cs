using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Infrastructure.Test.dummies
{
    public class HalfServiceMock : IHalfServiceMock
    {
        public bool TestCommandHandlerHasBeenCalled = false;
        public TestCommand TestCommandHandlerReceivedSomeCommand = null;
        public bool SomeCommandHandlerHasBeenCalled = false;
        public SomeCommand SomeCommandHandlerReceivedSomeCommand = null;

        public void TestCommandHandler(TestCommand command)
        {
            TestCommandHandlerHasBeenCalled = true;
            TestCommandHandlerReceivedSomeCommand = command;
        }

        public void SomeCommandHandler(SomeCommand command)
        {
            SomeCommandHandlerHasBeenCalled = true;
            SomeCommandHandlerReceivedSomeCommand = command;
        }
    }
}
