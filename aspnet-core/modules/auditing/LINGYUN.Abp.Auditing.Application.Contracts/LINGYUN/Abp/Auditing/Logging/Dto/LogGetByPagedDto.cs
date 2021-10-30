using Microsoft.Extensions.Logging;
using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Auditing.Logging
{
    public class LogGetByPagedDto : PagedAndSortedResultRequestDto
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public LogLevel? Level { get; set; }
        public string MachineName { get; set; }
        public string Environment { get; set; }
        public string Application { get; set; }
        public string Context { get; set; }
        public string RequestId { get; set; }
        public string RequestPath { get; set; }
        public string CorrelationId { get; set; }
        public int? ProcessId { get; set; }
        public int? ThreadId { get; set; }
        public bool? HasException { get; set; }
    }
}
