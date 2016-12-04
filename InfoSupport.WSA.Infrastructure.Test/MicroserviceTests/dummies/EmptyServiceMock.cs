﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Infrastructure.Test.dummies
{
    public class EmptyServiceMock : IEmptyServiceMock
    {
        public bool SomeCommandHandlerHasBeenCalled = false;

        public void SomeCommandHandler(SomeCommand command)
        {
            SomeCommandHandlerHasBeenCalled = true;
        }
    }
}
