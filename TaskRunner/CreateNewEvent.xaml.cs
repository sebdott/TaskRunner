using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskRunner.Events;
using TaskRunner.Misc;

namespace TaskRunner
{
    public partial class CreateNewEvent : Window
    {
        Main _mainwindow;
        public CreateNewEvent(Main mainwindow)
        {
            InitializeComponent();

            _mainwindow = mainwindow;
        }

        #region Click Events
        private void btnCopySourcePathBrowse_Click(object sender, RoutedEventArgs e)
        {
            txtCopySourcePath.AppendText(ReturnFolderNameBrowse());
        }

        private void btnCopyDestinationPath_Click(object sender, RoutedEventArgs e)
        {
            txtCopyDestinationPath.AppendText(ReturnFolderNameBrowse());
        }

        private void btnCreateEvent_Click(object sender, RoutedEventArgs e)
        {
            TabItem tab = tcNewEvent.SelectedItem as TabItem;

            if (IsValidated(tab.Name))
            {
                var eventInd = new Event()
                {
                    Id = Guid.NewGuid(),
                    Name = txtName.Text,
                    Details = txtDetails.Text
                };

                switch (tab.Name)
                {
                    case "tbCopy":
                        eventInd.Type = EventTypeEnum.Copy;
                        eventInd.CopyEvent = new CopyEvent()
                        {
                            SourcePath = txtCopySourcePath.Text,
                            DestinationPath = txtCopyDestinationPath.Text,

                        };

                        eventInd.CopyEvent.IsDirectCopy = chkIsDirectCopy.IsChecked.HasValue ? chkIsDirectCopy.IsChecked.Value : false;
                        eventInd.CopyEvent.IsReplaceExisting = chkIsReplaceExisting.IsChecked.HasValue ? chkIsReplaceExisting.IsChecked.Value : false;

                        var filePatterns = txtCopyFilePattern.Text.Split(';');

                        eventInd.CopyEvent.FilePatterns = filePatterns.ToList();

                        var folderPatterns = txtCopyFolderPattern.Text.Split(';');

                        eventInd.CopyEvent.FolderPatterns = folderPatterns.ToList();

                        break;
                    case "tbBuild":
                        eventInd.Type = EventTypeEnum.Build;
                        eventInd.BuildEvent = new BuildEvent()
                        {
                            SolutionPath = txtBuildSolutionPath.Text
                        };
                        break;
                    case "tbPowershell":
                        eventInd.Type = EventTypeEnum.Powershell;
                        eventInd.PowershellEvent = new PowershellEvent()
                        {
                            Message = txtPowershellMessage.Text
                        };
                        break;
                }

                if (AppResource.ListofEvents == null || AppResource.ListofEvents.ListofEvents.Count <= 0)
                {
                    var listofEvent = new List<Event>();
                    listofEvent.Add(eventInd);
                    AppResource.ListofEvents = new EventList()
                    {
                        ListofEvents = listofEvent
                    };
                }
                else
                {
                    AppResource.ListofEvents.ListofEvents.Add(eventInd);
                }

                _mainwindow.dgEvents.ItemsSource = AppResource.ListofEvents.ListofEvents;
                _mainwindow.dgEvents.Items.Refresh();

                this.Close();
            }
        }

        private void btnBuildSolutionPathBrowse_Click(object sender, RoutedEventArgs e)
        {
            txtBuildSolutionPath.Text = ReturnFileNameBrowse("Solution file|*.sln");
        } 
        #endregion

        #region Helper Method
        private string ReturnFileNameBrowse(string filter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return string.Empty;
        }

        private string ReturnFolderNameBrowse()
        {
            var openFolderDialog = new CommonOpenFileDialog();
            openFolderDialog.IsFolderPicker = true;

            var result = openFolderDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                return openFolderDialog.FileName;
            }

            return string.Empty;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            _mainwindow.MenuCreateEvent.IsEnabled = true;
        }

        private bool IsValidated(string tabName)
        {
            var result = true;

            result = !string.IsNullOrWhiteSpace(txtName.Text);

            switch (tabName)
            {
                case "tbCopy":
                    result = !string.IsNullOrWhiteSpace(txtCopySourcePath.Text);
                    result = !string.IsNullOrWhiteSpace(txtCopyDestinationPath.Text);
                    result = !string.IsNullOrWhiteSpace(txtCopyFilePattern.Text);
                    result = !string.IsNullOrWhiteSpace(txtCopyFolderPattern.Text);

                    break;
                case "tbBuild":
                    result = !string.IsNullOrWhiteSpace(txtBuildSolutionPath.Text);

                    break;
                case "tbPowershell":
                    result = !string.IsNullOrWhiteSpace(txtPowershellMessage.Text);

                    break;
            }

            return result;

        } 
        #endregion
    }
}
