namespace SutekiTmp.Domain.Common.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class LoggedAttribute : Attribute
    {
        public LogEvent Event { get; set; }
        public string ControllerTag { get; set; }

        public string ActionTag { get; set; }

        public LoggedAttribute(string ct, string at, LogEvent ev = LogEvent.None)
        {
            Event = ev;
            ControllerTag = ct;
            ActionTag = at;
        }
    }

    public enum LogEvent
    {
        None,
        Auth
    }
}
