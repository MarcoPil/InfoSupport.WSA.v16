﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Infrastructure.Test.dummies
{
    [Microservice("microserviceQueue")]
    interface ISomeMicroserviceMock
    {
        void TestCommandHandler(TestCommand command);
        void SomeCommandHandler(SomeCommand command);
    }
}
