namespace LINGYUN.Abp.Logging
{
    public class LogField
    {
        public string Context { get; set; }
        public string ActionId { get; set; }
        public string ActionName { get; set; }
        public string RequestId { get; set; }
        public string RequestPath { get; set; }
        public string ConnectionId { get; set; }
        public string CorrelationId { get; set; }
        public string ClientId { get; set; }
        public string UserId { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
    }
}
