using Microsoft.Extensions.Logging;
using Serilog.Formatting.Elasticsearch;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch
{
    [Serializable]
    public class SerilogInfo
    {
        [Nest.PropertyName(ElasticsearchJsonFormatter.TimestampPropertyName)]
        public DateTime TimeStamp { get; set; }

        [Nest.PropertyName(ElasticsearchJsonFormatter.LevelPropertyName)]
        public LogLevel Level { get; set; }

        [Nest.PropertyName(ElasticsearchJsonFormatter.RenderedMessagePropertyName)]
        public string Message { get; set; }

        [Nest.PropertyName("fields")]
        public SerilogField Fields { get; set; }

        [Nest.PropertyName("exceptions")]
        public List<SerilogException> Exceptions { get; set; }
    }
}
