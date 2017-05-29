using System.ComponentModel;

namespace TaskRunner.Misc
{
    public enum EventTypeEnum
    {
        [Description("Do Nothing")]
        Default,
        [Description("Copy")]
        Copy,
        [Description("Build")]
        Build,
        [Description("Powershell")]
        Powershell
    }
}
