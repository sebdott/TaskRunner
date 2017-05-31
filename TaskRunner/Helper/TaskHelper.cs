using TaskRunner.Handler;
using TaskRunner.Events;
using TaskRunner.EventHandler;
using System;
using TaskRunner.Misc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace TaskRunner.Helper
{
    public class TaskHelper
    {
        public static void Execute(Event input)
        {
            var handler = ResolveHandler(input);

            if (handler != null)
            {
                handler.Execute();
            }
         }

        private static IEventHandler ResolveHandler(Event type)
        {
            IUnityContainer container = new UnityContainer();
            var config = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            container.LoadConfiguration(config);

            var enumType = Util.GetEnumValue(type.Type);

            return container.Resolve<IEventHandler>(enumType, new ResolverOverride[]
                                   {
                                       new ParameterOverride(enumType,type)
                                   });
        }
    }
}
