using System;
using System.Management.Automation;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TaskRunner.Events
{
    public class Build : Event
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
   

        [XmlIgnore]
        public override string DetailsDescription
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine("Solution Path: " + SolutionPath);

                return sb.ToString();
            }
        }
    }


}
