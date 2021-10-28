using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Logging
{
    public class LogInfo
    {
        public DateTime TimeStamp { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public LogField Fields { get; set; }
        public List<LogException> Exceptions { get; set; }
    }
}
