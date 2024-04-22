using AutoMapper;
using Serilog.Events;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch
{
    public class AbpLoggingSerilogElasticsearchMapperProfile : Profile
    {
        public AbpLoggingSerilogElasticsearchMapperProfile()
        {
            CreateMap<SerilogException, LogException>();
            CreateMap<SerilogField, LogField>()
                .ForMember(log => log.Id, map => map.MapFrom(slog => slog.UniqueId.ToString()));
            CreateMap<SerilogInfo, LogInfo>()
                .ForMember(log => log.Level, map => map.MapFrom(slog => GetLogLevel(slog.Level)));
        }

        private static Microsoft.Extensions.Logging.LogLevel GetLogLevel(LogEventLevel logEventLevel)
        {
            return logEventLevel switch
            {
                LogEventLevel.Fatal => Microsoft.Extensions.Logging.LogLevel.Critical,
                LogEventLevel.Error => Microsoft.Extensions.Logging.LogLevel.Error,
                LogEventLevel.Warning => Microsoft.Extensions.Logging.LogLevel.Warning,
                LogEventLevel.Information => Microsoft.Extensions.Logging.LogLevel.Information,
                LogEventLevel.Debug => Microsoft.Extensions.Logging.LogLevel.Debug,
                _ => Microsoft.Extensions.Logging.LogLevel.Trace,
            };
        }
    }
}
