using System;
using TaskRunner.Events;
using TaskRunner.Properties;
using TaskRunner.EventHandler;
using System.Management.Automation;
using System.Threading;

namespace TaskRunner.Handler
{
    public class PowershellHandler : IEventHandler<PowershellRun>
    {
        private readonly int timeout;
        private readonly string powershellScriptPath;
        private readonly string cakeScriptPath;
        private TimeSpan timeoutTS;
        private const char ExtensionDelimer = ';';

        public event EventHandler<EventArgs> IsCompleted;
        public event EventHandler<EventArgs> IsFailed;

        public PowershellHandler()
        {
            timeout = Settings.Default.ExecutionTimeOutInSeconds;
            powershellScriptPath = Settings.Default.PowershellScriptPath;
            cakeScriptPath = Settings.Default.CakeScriptPath;
            timeoutTS = new TimeSpan(0, 0, 0, timeout);
        }

        public void Execute(PowershellRun runPowershellInput)
        {
            IsCompleted += runPowershellInput.IsCompleted;
            IsFailed += runPowershellInput.IsFailed;

            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript(runPowershellInput.Message);

                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += runPowershellInput.OutputCollection_DataAdded;

                PowerShellInstance.Streams.Error.DataAdded += runPowershellInput.Error_DataAdded;

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
