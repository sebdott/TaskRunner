using System;
using System.Management.Automation;
using System.Threading;
using TaskRunner.Events;
using TaskRunner.Properties;

namespace TaskRunner.Handler
{
    public class Build : IEventHandler<BuildEventInput>
    {

        private readonly int timeout;
        private readonly string powershellScriptPath;
        private readonly string cakeScriptPath;
        private TimeSpan timeoutTS;
        private const char ExtensionDelimer = ';';

        public event EventHandler<EventArgs> IsCompleted;

        public event EventHandler<EventArgs> IsFailed;

        public Build()
        {
            timeout = Settings.Default.ExecutionTimeOutInSeconds;
            powershellScriptPath = Settings.Default.PowershellScriptPath;
            cakeScriptPath = Settings.Default.CakeScriptPath;
            timeoutTS = new TimeSpan(0, 0, 0, timeout);
        }

        public void Execute(BuildEventInput buildEventInput)
        {

            IsCompleted += buildEventInput.IsCompleted;
            IsFailed += buildEventInput.IsFailed;

            using (PowerShell PowerShellInstance = PowerShell.Create())
            {

                PowerShellInstance.AddScript("Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass");
                PowerShellInstance.AddScript(@"" + powershellScriptPath + @" -Script " + cakeScriptPath
                + " -solutionPath=\"" + buildEventInput.SolutionPath + "\"");


                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += buildEventInput.OutputCollection_DataAdded;

                PowerShellInstance.Streams.Error.DataAdded += buildEventInput.Error_DataAdded;

                DateTime startTime = DateTime.Now;

                IAsyncResult result = PowerShellInstance.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                while (result.IsCompleted == false)
                {
                    Thread.Sleep(1000);

                    TimeSpan elasped = DateTime.Now.Subtract(startTime);
                    if (elasped > timeoutTS)
                    {
                        OnFailed();
                        break;
                    }
                }

                if (result.IsCompleted)
                {
                    OnCompleted();
                }
            }
        }

        protected virtual void OnCompleted()
        {
            IsCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFailed()
        {
            IsFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
