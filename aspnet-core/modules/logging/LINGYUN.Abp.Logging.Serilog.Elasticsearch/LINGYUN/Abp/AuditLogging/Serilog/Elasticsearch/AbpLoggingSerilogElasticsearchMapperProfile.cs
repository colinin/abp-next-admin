using AutoMapper;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch
{
    public class AbpLoggingSerilogElasticsearchMapperProfile : Profile
    {
        public AbpLoggingSerilogElasticsearchMapperProfile()
        {
            CreateMap<SerilogException, LogException>();
            CreateMap<SerilogField, LogField>()
                .ForMember(log => log.Id, map => map.MapFrom(slog => slog.UniqueId.ToString()));
            CreateMap<SerilogInfo, LogInfo>();
        }
    }
}
