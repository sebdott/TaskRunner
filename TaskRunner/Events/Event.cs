using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using TaskRunner.Misc;
using System.Text;

namespace TaskRunner.Events
{
    [Serializable]
    [XmlRoot("Events")]
    public class EventList
    {
        [XmlElement("Event")]
        public List<Event> ListofEvents { get; set; }

    }
    public class Event : INotifyPropertyChanged
    {
        public Event()
        {
            _status = StatusEnum.Pending;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public EventTypeEnum Type { get; set; }

        public string Details { get; set; }

        public int Order { get; set; }

        public BuildEvent BuildEvent { get; set; }

        public CopyEvent CopyEvent { get; set; }

        public PowershellEvent PowershellEvent { get; set; }
        
        private StatusEnum _status;
        public event PropertyChangedEventHandler PropertyChanged;

        public StatusEnum Status {

            get {

                return _status;
            }
            set {

                _status = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }
        
        [XmlIgnore]
        public string DetailsDescription
        {
            get
            {
                var sb = new StringBuilder();

                switch (Type)
                {
                    case EventTypeEnum.Build:
                        sb.AppendLine("Solution Path: " + BuildEvent.SolutionPath);
                        break;
                    case EventTypeEnum.Copy:
                        sb.AppendLine("Is Direct Copy: " + (CopyEvent.IsDirectCopy ? "Yes" : "No"));
                        sb.AppendLine("Source Path: " + CopyEvent.SourcePath.ToString());
                        sb.AppendLine("File Copy Pattern: " + string.Join(",", CopyEvent.FilePatterns));
                        sb.AppendLine("Folder Copy Pattern: " + string.Join(",", CopyEvent.FolderPatterns));
                        sb.AppendLine("Destination Path: " + CopyEvent.DestinationPath.ToString());
                        break;
                    case EventTypeEnum.Powershell:
                        sb.AppendLine("Message: " + PowershellEvent.Message);
                        break;
                }

                return sb.ToString();
            }
            set
            {
                DetailsDescription = value;
            }
        }
    }
}
