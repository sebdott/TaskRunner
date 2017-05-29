using TaskRunner.Handler;
using TaskRunner.Events;
using TaskRunner.EventHandler;

namespace TaskRunner.Helper
{
    public class TaskHelper
    {
        public static void Build(BuildEventInput buildEventInput)
        {
            var eventDisPatcher = new EventDispatcher<BuildEventInput>(buildEventInput);
            eventDisPatcher.Execute(new Build());
        }

        public static void Copy(CopyEventInput copyEventInput)
        {
            var eventDisPatcher = new EventDispatcher<CopyEventInput>(copyEventInput);
            eventDisPatcher.Execute(new Copy());
        }
    }
}
