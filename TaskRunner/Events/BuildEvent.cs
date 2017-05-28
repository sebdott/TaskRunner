using System.Xml.Serialization;

namespace TaskRunner.Events
{
    [XmlInclude(typeof(Event))]
    public class BuildEvent : Event
    {
        public string SolutionPath { get; set; }
    }


}
