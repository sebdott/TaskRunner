using System;
using System.Management.Automation;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TaskRunner.Events
{
    public class PowershellRun : Event
    {
        public string Message { get; set; }
        [XmlIgnore]
        public EventHandler<DataAddedEventArgs> OutputCollection_DataAdded { get; set; }
        [XmlIgnore]
        public EventHandler<DataAddedEventArgs> Error_DataAdded { get; set; }
        [XmlIgnore]
        public EventHandler<EventArgs> IsCompleted { get; set; }
        [XmlIgnore]
        public EventHandler<EventArgs> IsFailed { get; set; }

        [XmlIgnore]
        public override string DetailsDescription
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine("Message: " + Message);

                return sb.ToString();
            }
        }
    }
}
