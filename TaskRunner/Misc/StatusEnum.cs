using System.ComponentModel;

namespace TaskRunner.Misc
{
    public enum StatusEnum
    {
        [Description("Success")]
        Success,
        [Description("Failed")]
        Failed,
        [Description("Pending")]
        Pending,
        [Description("Running")]
        Running
    }
}
