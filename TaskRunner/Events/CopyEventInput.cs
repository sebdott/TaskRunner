using System;
using TaskRunner.Handler;

namespace TaskRunner.Events
{
    public class CopyEventInput :Copy
    { 
        public EventHandler<CopyEventArgs> IsCompleted { get; set; }

        public EventHandler<EventArgs> IsFailed { get; set; }

    }
}
