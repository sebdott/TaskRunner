using System.Xml;
using System.Xml.Serialization;

namespace TaskRunner.Events
{
    [XmlInclude(typeof(Event))]
    public class PowershellEvent : Event
    {
        public string Message { get; set; }
    }
}
