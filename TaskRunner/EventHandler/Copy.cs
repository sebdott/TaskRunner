using System;
using System.IO;
using System.Linq;
using TaskRunner.Events;
using TaskRunner.Properties;
using TaskRunner.Misc;
using System.Collections.Generic;

namespace TaskRunner.Handler
{
    public class Copy : IEventHandler<CopyEventInput>
    {

        private readonly int timeout;
        private readonly string powershellScriptPath;
        private readonly string cakeScriptPath;
        private TimeSpan timeoutTS;
        private const char ExtensionDelimer = ';';

        public event EventHandler<CopyEventArgs> IsCompleted;
        public event EventHandler<EventArgs> IsFailed;

        public Copy()
        {
            timeout = Settings.Default.ExecutionTimeOutInSeconds;
            powershellScriptPath = Settings.Default.PowershellScriptPath;
            cakeScriptPath = Settings.Default.CakeScriptPath;
            timeoutTS = new TimeSpan(0, 0, 0, timeout);
        }

        public void Execute(CopyEventInput copyEventInput)
        {
            IsCompleted += copyEventInput.IsCompleted;
            IsFailed += copyEventInput.IsFailed;

            var packageFolder = copyEventInput.DestinationPath;

            if (copyEventInput.IsDirectCopy)
            {
                Util.Copy(copyEventInput.SourcePath, copyEventInput.DestinationPath);
            }
            else
            {
                foreach (var folder in copyEventInput.FolderPatterns)
                {
                    var folderPatternDirectories = new List<string>();

                    if (folder == "*")
                    {
                        folderPatternDirectories.Add(copyEventInput.SourcePath);
                    }
                    else
                    {
                        folderPatternDirectories = Directory.GetDirectories(copyEventInput.SourcePath, folder, SearchOption.AllDirectories).ToList();
                    }

                    foreach (var folderPatternDirectory in folderPatternDirectories)
                    {
                        var listofFilePath = new List<string>();

                        foreach (var sourcePattern in copyEventInput.FilePatterns)
                        {
                            listofFilePath.AddRange(Directory.GetFiles(folderPatternDirectory, "*" + sourcePattern, System.IO.SearchOption.AllDirectories).ToList());
                        }

                        foreach (var filePath in listofFilePath)
                        {
                            var destinationPath = copyEventInput.DestinationPath + "\\" + Path.GetFileName(filePath);

                            File.Copy(filePath, destinationPath, true);
                            OnCompleted(filePath, destinationPath);
                        }
                    }
                }
            }
        }

        protected virtual void OnCompleted(string sourceFilePath, string destinationFilePath)
        {
            IsCompleted?.Invoke(this, new CopyEventArgs() { SourceFilePath = sourceFilePath, DestinationFilePath = destinationFilePath });
        }

        protected virtual void OnFailed()
        {
            IsFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class CopyEventArgs : EventArgs
    {
        public string SourceFilePath { get; set; }
        public string DestinationFilePath { get; set; }
    }
}
