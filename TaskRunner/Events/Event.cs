using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using TaskRunner.Misc;
using System.Text;

namespace TaskRunner.Events
{
    //[Serializable]
    //[XmlRoot("EventTask")]
    //public class EventTask
    //{
    //    [XmlArray(ElementName = "Events")]
    //    [XmlArrayItem(ElementName = "Event")]
    //    public List<Event> ListofEvents { get; set; }

    //}

    [XmlRoot("Events")]
    public class EventTask : List<Event>
    {

    }
    
    [XmlInclude(typeof(Build))]
    [XmlInclude(typeof(Copy))]
    [XmlInclude(typeof(PowershellRun))]
    public abstract class Event : INotifyPropertyChanged
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
        public virtual string DetailsDescription
        {
            get
            {
                var sb = new StringBuilder();

                return sb.ToString();
            }
        }
    }
}
