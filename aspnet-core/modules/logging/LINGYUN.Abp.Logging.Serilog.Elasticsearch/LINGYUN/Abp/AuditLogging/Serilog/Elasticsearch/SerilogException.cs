namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch
{
    public class SerilogException
    {
        [Nest.PropertyName("SourceContext")]
        public int Depth { get; set; }

        [Nest.PropertyName("ClassName")]
        public string Class { get; set; }

        [Nest.PropertyName("Message")]
        public string Message { get; set; }

        [Nest.PropertyName("Source")]
        public string Source { get; set; }

        [Nest.PropertyName("StackTraceString")]
        public string StackTrace { get; set; }

        [Nest.PropertyName("HResult")]
        public int HResult { get; set; }

        [Nest.PropertyName("HelpURL")]
        public string HelpURL { get; set; }
    }
}
