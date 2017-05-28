using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace TaskRunner.Events
{
    [XmlInclude(typeof(Event))]
    public class CopyEvent : Event
    {
        public string SourcePath { get; set; }

        [XmlArray]
        [XmlArrayItem("FilePattern")]
        public List<string> FilePatterns { get; set; }
        [XmlArray]
        [XmlArrayItem("FolderPattern")]
        public List<string> FolderPatterns { get; set; }

        [XmlIgnore]
        public Boolean IsDirectCopy { get; set; }

        [XmlElement("IsDirectCopy")]
        public string IsDirectCopySerialize
        {
            get { return this.IsDirectCopy ? "True" : "False"; }
            set
            {
                if (value.ToLower().Equals("true"))
                    this.IsDirectCopy = true;
                else if (value.ToLower().Equals("false"))
                    this.IsDirectCopy = false;
                else
                    this.IsDirectCopy = XmlConvert.ToBoolean(value);
            }
        }
        public string DestinationPath { get; set; }


    }
}
