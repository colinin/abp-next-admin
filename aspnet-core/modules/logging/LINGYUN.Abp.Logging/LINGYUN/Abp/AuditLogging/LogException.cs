namespace LINGYUN.Abp.Logging
{
    public class LogException
    {
        public int Depth { get; set; }
        public string Class { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public int HResult { get; set; }
        public string HelpURL { get; set; }
    }
}
