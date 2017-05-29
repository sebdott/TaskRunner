using System.Xml;
using System.Xml.Serialization;

namespace TaskRunner.Events
{
    [XmlInclude(typeof(Event))]
    public class BuildEvent : Event
    {
        public string SolutionPath { get; set; }

        [XmlIgnore]
        public bool IsIISReset { get; set; }

        [XmlElement("IsIISReset")]
        public string IsIISResetSerialize
        {
            get { return this.IsIISReset ? "True" : "False"; }
            set
            {
                if (value.ToLower().Equals("true"))
                    this.IsIISReset = true;
                else if (value.ToLower().Equals("false"))
                    this.IsIISReset = false;
                else
                    this.IsIISReset = XmlConvert.ToBoolean(value);
            }
        }
    }


}
