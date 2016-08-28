using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSupport.WSA.Common
{
    /// <summary>
    /// Implementers should Execute commands and be able to receive FunctionalExceptions
    /// </summary>
    public interface ICommander : IDisposable
    {
        void Execute(Command command);
        Task ExecuteAsync(Command command);
    }
}
