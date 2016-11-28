using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Infrastructure.Test.dummies
{
    public class OtherMicroserviceMock : IOtherMicroserviceMock
    {
        public bool SomeCommandHandlerHasBeenCalled = false;
        public SomeCommand SomeCommandHandlerReceivedSomeCommand = null;

        public void SomeCommandHandler(SomeCommand command)
        {
            SomeCommandHandlerHasBeenCalled = true;
            SomeCommandHandlerReceivedSomeCommand = command;
        }
    }
}
