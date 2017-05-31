using System;
using TaskRunner.Events;

namespace TaskRunner.Handler
{
    public interface IEventHandler<TEventInput> where TEventInput : Event
    {
        void Execute(TEventInput evenInput);
    }
}
