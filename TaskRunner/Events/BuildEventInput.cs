using System;
using System.Management.Automation;

namespace TaskRunner.Events
{
    public class BuildEventInput : BuildEvent
    {
        public EventHandler<DataAddedEventArgs> OutputCollection_DataAdded { get; set; }

        public EventHandler<DataAddedEventArgs> Error_DataAdded { get; set; }
        
        public EventHandler<EventArgs> IsCompleted { get; set; }

        public EventHandler<EventArgs> IsFailed { get; set; }

    }
}
