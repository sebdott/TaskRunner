using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TaskRunner.Handler;

namespace TaskRunner.Events
{
    public class Copy : Event
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

        [XmlIgnore]
        public Boolean IsReplaceExisting { get; set; }

        [XmlElement("IsReplaceExisting")]
        public string IsReplaceExistingSerialize
        {
            get { return this.IsReplaceExisting ? "True" : "False"; }
            set
            {
                if (value.ToLower().Equals("true"))
                    this.IsReplaceExisting = true;
                else if (value.ToLower().Equals("false"))
                    this.IsReplaceExisting = false;
                else
                    this.IsReplaceExisting = XmlConvert.ToBoolean(value);
            }
        }

        public string DestinationPath { get; set; }
        [XmlIgnore]
        public EventHandler<CopyEventArgs> IsCompleted { get; set; }

        [XmlIgnore]
        public override string DetailsDescription
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine("Is Direct Copy: " + (IsDirectCopy ? "Yes" : "No"));
                sb.AppendLine("Source Path: " + SourcePath.ToString());
                sb.AppendLine("File Copy Pattern: " + string.Join(",", FilePatterns));
                sb.AppendLine("Folder Copy Pattern: " + string.Join(",", FolderPatterns));
                sb.AppendLine("Destination Path: " + DestinationPath.ToString());

                return sb.ToString();
            }
        }
    }
}
