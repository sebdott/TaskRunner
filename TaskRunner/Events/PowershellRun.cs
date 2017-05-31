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
