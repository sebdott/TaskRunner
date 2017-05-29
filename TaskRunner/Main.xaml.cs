using Microsoft.Win32;
using System;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskRunner.Events;
using TaskRunner.Handler;
using TaskRunner.Helper;
using TaskRunner.Misc;

namespace TaskRunner
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            
        }
        
        #region Click Events
        private async void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            dgEvents.IsEnabled = false;

            int index = 0;

            if (AppResource.ListofEvents == null)
            {
                Dispatcher.Invoke((Action)(() =>
                {
                    UpdateLblStatus("Warning: There is no record in the event list !");
                }));
                return;
            }

            foreach (var eventInd in AppResource.ListofEvents.ListofEvents)
            {
                Action action = null;

                UpdateLblStatus("Task : "+ eventInd.Name+ " Processing...... ");

                switch (eventInd.Type)
                {
                    case EventTypeEnum.Build:

                        action = () =>
                        {
                            TaskHelper.Build(new BuildEventInput()
                            {
                                SolutionPath = eventInd.BuildEvent.SolutionPath,
                                OutputCollection_DataAdded = OutputCollection_DataAdded,
                                Error_DataAdded = Error_DataAdded,
                                IsCompleted = IsBuildCompleted,
                                IsFailed = IsBuildFailed

                            });
                        };
                        break;

                    case EventTypeEnum.Copy:

                        action = () =>
                        {
                            TaskHelper.Copy(new CopyEventInput()
                            {
                                IsDirectCopy =eventInd.CopyEvent.IsDirectCopy,
                                SourcePath = eventInd.CopyEvent.SourcePath,
                                FilePatterns = eventInd.CopyEvent.FilePatterns,
                                DestinationPath = eventInd.CopyEvent.DestinationPath,
                                FolderPatterns = eventInd.CopyEvent.FolderPatterns,
                                IsCompleted = IsCopyCompleted,
                                IsFailed = IsBuildFailed,
                                IsReplaceExisting = eventInd.CopyEvent.IsReplaceExisting,
                            });

                        };
                        break;
                }

                if (action != null)
                {
                    dgEvents.SelectedIndex = index;

                    eventInd.Status = StatusEnum.Running;
                    dgEvents.SelectedValue = eventInd;
                    await Task.Run(() => action());

                    eventInd.Status = StatusEnum.Success;
                    dgEvents.SelectedValue = eventInd;
                    index++;
                }
            }

            dgEvents.IsEnabled = true;
            
            UpdateLblStatus("Notice: Process Completed !");

        }

        private void CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            CreateNewEvent createNewEvent = new CreateNewEvent(this);
            createNewEvent.Show();
            MenuCreateEvent.IsEnabled = false;
         //   this.Close();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files|*.config";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                var listofEvent = Util.LoadXML<EventList>(openFileDialog);

                AppResource.ListofEvents = listofEvent;
                AppResource.CurrentFile = openFileDialog.FileName;

                dgEvents.ItemsSource = AppResource.ListofEvents.ListofEvents;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Util.Save(AppResource.CurrentFile, AppResource.ListofEvents);
                lblStatus.Content = "Save Success";
            }
            catch
            {
                lblStatus.Content = "Save Fail, Please try again later";
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Config files|*.config";

                if (saveFileDialog.ShowDialog() == true)
                {
                    Util.Save(saveFileDialog.FileName, AppResource.ListofEvents);
                    AppResource.CurrentFile = saveFileDialog.FileName;
                }

                lblStatus.Content = "Save Success";
            }
            catch
            {
                lblStatus.Content = "Save Fail, Please try again later";

            }

        }
        #endregion

        #region Handlers
        
        private void DriversDataGrid_PreviewDeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == DataGrid.DeleteCommand)
            {
                if (!(MessageBox.Show("Are you sure you want to delete?", "Please confirm.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    // Cancel Delete.
                    e.Handled = true;
                }
            }
        }

        public void IsCopyCompleted(object sender, CopyEventArgs e)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                txtOutput.AppendText(Environment.NewLine);
                txtOutput.AppendText("Success Copied From :" + e.SourceFilePath);
                txtOutput.AppendText(Environment.NewLine);
                txtOutput.AppendText("Success Copied To :" + e.DestinationFilePath);
                txtOutput.ScrollToEnd();
            }));
        }

        public void IsBuildFailed(object sender, EventArgs e)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                txtOutput.AppendText(Environment.NewLine);
                txtOutput.AppendText(Environment.NewLine);
                txtOutput.AppendText(Environment.NewLine);
                txtOutput.AppendText("Building script taking too long");
                txtOutput.ScrollToEnd();
            }));
        }

        public void IsBuildCompleted(object sender, EventArgs e)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                txtOutput.AppendText(Environment.NewLine);
                txtOutput.AppendText("--------------------------------------------------");
                txtOutput.AppendText(Environment.NewLine);
                txtOutput.AppendText("----- Build Success ----- ^_^ -----");
                txtOutput.AppendText(Environment.NewLine);
                txtOutput.ScrollToEnd();
            }));


        }

        public void OutputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            var outputList = (PSDataCollection<PSObject>)sender;

            if (outputList.Count > 0)
            {
                Dispatcher.Invoke((Action)(() =>
                {
                    txtOutput.AppendText(outputList[outputList.Count - 1].BaseObject.ToString());
                    txtOutput.AppendText(Environment.NewLine);
                    txtOutput.ScrollToEnd();
                }

                 ));
            }
        }

        public void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            var outputList = (PSDataCollection<ErrorRecord>)sender;

            Dispatcher.Invoke((Action)(() =>
            {
                txtOutput.AppendText(outputList[outputList.Count - 1].Exception.Message);
                txtOutput.AppendText(Environment.NewLine);
                txtOutput.ScrollToEnd();
            }

               ));
        }

        #endregion

        private void UpdateLblStatus(string message)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                lblStatus.Content = message;
            }));
        }
    }
}

