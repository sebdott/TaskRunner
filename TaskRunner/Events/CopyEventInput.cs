using System;
using TaskRunner.Handler;

namespace TaskRunner.Events
{
    public class CopyEventInput :CopyEvent
    { 
        public EventHandler<CopyEventArgs> IsCompleted { get; set; }

        public EventHandler<EventArgs> IsFailed { get; set; }

    }
}
