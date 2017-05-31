using System;
using TaskRunner.Events;

namespace TaskRunner.Handler
{
    public interface IEventHandler
    {
        void Execute();
    }
}
