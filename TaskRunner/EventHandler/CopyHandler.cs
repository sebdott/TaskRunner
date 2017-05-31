using System;
using System.IO;
using System.Linq;
using TaskRunner.Events;
using TaskRunner.Properties;
using TaskRunner.Misc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskRunner.EventHandler;

namespace TaskRunner.Handler
{
    public class CopyHandler : IEventHandler<Copy>
    {
        private readonly int timeout;
        private readonly string powershellScriptPath;
        private readonly string cakeScriptPath;
        private TimeSpan timeoutTS;
        private const char ExtensionDelimer = ';';

        public event EventHandler<CopyEventArgs> IsCompleted;
        public event EventHandler<EventArgs> IsFailed;

        public CopyHandler()
        {
            timeout = Settings.Default.ExecutionTimeOutInSeconds;
            powershellScriptPath = Settings.Default.PowershellScriptPath;
            cakeScriptPath = Settings.Default.CakeScriptPath;
            timeoutTS = new TimeSpan(0, 0, 0, timeout);
        }

        public void Execute(Copy copyEventInput)
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

                    foreach (var sourceFolderPatternDirectory in folderPatternDirectories)
                    {
                        var listofSourceFilePath = new List<string>();

                        foreach (var sourcePattern in copyEventInput.FilePatterns)
                        {
                            listofSourceFilePath.AddRange(Directory.GetFiles(sourceFolderPatternDirectory, "*" + sourcePattern, System.IO.SearchOption.AllDirectories).ToList());
                        }
                        
                        if (copyEventInput.IsReplaceExisting)
                        {
                            var taskList = new List<Task>();

                            foreach (var filePath in listofSourceFilePath)
                            {
                                var copyFileAction = Task.Run(() =>
                                {
                                    foreach (var ind in Directory.GetFiles(copyEventInput.DestinationPath, Path.GetFileName(filePath), System.IO.SearchOption.AllDirectories))
                                    {
                                        File.Copy(filePath, ind, true);
                                        OnCompleted(filePath, ind);
                                    }

                                });

                                taskList.Add(copyFileAction);
                            }

                            Task.WaitAll(taskList.ToArray());
                        }
                        else
                        {
                            foreach (var filePath in listofSourceFilePath)
                            {
                                var destinationPath = copyEventInput.DestinationPath + "\\" + Path.GetFileName(filePath);

                                File.Copy(filePath, destinationPath, true);
                                OnCompleted(filePath, destinationPath);
                            }
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
