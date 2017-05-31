using TaskRunner.Handler;
using TaskRunner.Events;
using TaskRunner.EventHandler;
using System;
using TaskRunner.Misc;

namespace TaskRunner.Helper
{
    public class TaskHelper
    {
        //public static void Build(BuildEventInput input)
        //{
        //    var eventDisPatcher = new EventDispatcher<BuildEventInput>(input);
        //    eventDisPatcher.Execute(new Handler.Build());
        //}

        //public static void Copy(CopyEventInput input)
        //{
        //    var eventDisPatcher = new EventDispatcher<CopyEventInput>(input);
        //    eventDisPatcher.Execute(new Handler.Copy());
        //}

        //public static void Powershell(PowershellEventInput input)
        //{
        //    var eventDisPatcher = new EventDispatcher<PowershellEventInput>(input);
        //    eventDisPatcher.Execute(new PowershellCommand());
        //}

        public static void Execute(Event input)
        {
            var t = Util.ResolveHandler<Event, IEventHandler<Event>>(Enum.GetName(typeof(EventTypeEnum), input.Type));

            var eventDisPatcher = new EventDispatcher<Event>(input);
            eventDisPatcher.Execute((IEventHandler<Event>)Activator.CreateInstance(typeof(IEventHandler<Event>)));
        }
    }
}
