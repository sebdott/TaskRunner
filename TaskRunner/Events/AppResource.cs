using System.Collections.Generic;
using System.Windows;

namespace TaskRunner.Events
{
    public static class AppResource
    {
        public static EventTask ListofEvents
        {
            get
            {
                return Application.Current.Resources["Events"] as EventTask;
                
            }
            set
            {
                Application.Current.Resources["Events"] = value;
            }
        }

        public static string CurrentFile
        {
            get
            {
                return Application.Current.Resources["CurrentFile"] as string;
            }
            set
            {
                Application.Current.Resources["CurrentFile"] = value;
            }
        }
    }
}
