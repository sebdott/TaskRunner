using TaskRunner.Handler;
using TaskRunner.Events;
using TaskRunner.EventHandler;

namespace TaskRunner.Helper
{
    public class TaskHelper
    {
        public static void Build(BuildEventInput input)
        {
            var eventDisPatcher = new EventDispatcher<BuildEventInput>(input);
            eventDisPatcher.Execute(new Build());
        }

        public static void Copy(CopyEventInput input)
        {
            var eventDisPatcher = new EventDispatcher<CopyEventInput>(input);
            eventDisPatcher.Execute(new Copy());
        }

        public static void Powershell(PowershellEventInput input)
        {
            var eventDisPatcher = new EventDispatcher<PowershellEventInput>(input);
            eventDisPatcher.Execute(new PowershellCommand());
        }
    }
}
