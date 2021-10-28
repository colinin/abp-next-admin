using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Auditing.Logging
{
    public class LogDto
    {
        public DateTime TimeStamp { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public LogFieldDto Fields { get; set; }
        public List<LogExceptionDto> Exceptions { get; set; }
    }
}
