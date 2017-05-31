using Microsoft.Win32;
using System;
using System.Diagnostics;
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
            btnExecute.IsEnabled = false;

            int index = 0;

            if (AppResource.ListofEvents == null)
            {
                UpdateLblStatus("Warning: There is no record in the event list !");

                return;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var eventInd in AppResource.ListofEvents)
            {
                Action action = null;

                UpdateLblStatus("Task : " + eventInd.Name + " Processing...... ");

                action = () =>
                        {
                            eventInd.OutputCollection_DataAdded = OutputCollection_DataAdded;
                            eventInd.Error_DataAdded = Error_DataAdded;
                            eventInd.IsCompleted = IsBuildCompleted;
                            eventInd.IsFailed = IsBuildFailed;

                            if (eventInd.Type == EventTypeEnum.Copy)
                            {
                                var copyEvent = ((Copy)eventInd);

                                copyEvent.IsCompleted = IsCopyCompleted;
                                TaskHelper.Execute(eventInd);
                            }
                            else {

                                TaskHelper.Execute(eventInd);
                            }
                        };
                

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
            btnExecute.IsEnabled = true;
            sw.Stop();
            UpdateLblStatus("Notice: Process Completed ! Total Run Time :" + sw.Elapsed);

           
            
        }

        private void CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            //CreateNewEvent createNewEvent = new CreateNewEvent(this);
            //createNewEvent.Show();
            MenuCreateEvent.IsEnabled = false;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files|*.config";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                var eventTask = Util.LoadXML<EventTask>(openFileDialog);

                AppResource.ListofEvents = eventTask;
                AppResource.CurrentFile = openFileDialog.FileName;

                dgEvents.ItemsSource = AppResource.ListofEvents;
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
                txtOutput.AppendText(txtOutput.Text.Length +"Success Copied To :" + e.DestinationFilePath);
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

        #region Helper Method
        private void UpdateLblStatus(string message)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                lblStatus.Content = message;
            }));
        }
        #endregion
    }
}

