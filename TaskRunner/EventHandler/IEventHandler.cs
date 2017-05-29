using System;

namespace TaskRunner.EventHandler
{
    public interface IEventHandler<TEventInput>
    {
        void Execute(TEventInput evenInput);
    }
}
