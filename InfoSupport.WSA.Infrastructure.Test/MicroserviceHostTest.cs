using InfoSupport.WSA.Infrastructure.Test.dummies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InfoSupport.WSA.Infrastructure.Test
{
    public class MicroserviceHostTest
    {
        [Fact]
        public void MicroserviceHostFindsHandlers()
        {
            using (var host = new MicroserviceHost<SomeMicroserviceMock>())
            {
                Assert.Equal(2, host.ServiceModel.Handlers.Count());
                Assert.True(host.ServiceModel.Handlers.Contains("InfoSupport.WSA.Infrastructure.Test.dummies.TestCommand"), "TestCommand is not recognized");
                Assert.True(host.ServiceModel.Handlers.Contains("InfoSupport.WSA.Infrastructure.Test.dummies.SomeCommand"), "SomeCommand is not recognized");
            }
        }
    }
}
