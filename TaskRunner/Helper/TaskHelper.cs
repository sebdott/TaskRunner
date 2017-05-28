using System;
using System.Windows;
using System.Management.Automation;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Win32;
using System.Threading;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using TaskRunner.Properties;
using System.Windows.Controls;
using System.Windows.Threading;
using TaskRunner.Handler;
using TaskRunner.Events;
using System.Threading.Tasks;

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
