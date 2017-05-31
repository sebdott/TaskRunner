using System;
using TaskRunner.Events;
using TaskRunner.Handler;

namespace TaskRunner.EventHandler
{
    public class EventDispatcher<TEventInput> where TEventInput : Event
    {
        public TEventInput _eventInput;

        public EventDispatcher(TEventInput EventInput)
        {
            _eventInput = EventInput;
        }

        public void Execute(IEventHandler<TEventInput> EventHandler)
        {
            if (EventHandler != null)
            {
                EventHandler.Execute(_eventInput);
            }
            else
            {
                throw new Exception("no handler registered");
            }
        }
    }
}
