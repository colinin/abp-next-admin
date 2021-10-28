using AutoMapper;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch
{
    public class AbpLoggingSerilogElasticsearchMapperProfile : Profile
    {
        public AbpLoggingSerilogElasticsearchMapperProfile()
        {
            CreateMap<SerilogException, LogException>();
            CreateMap<SerilogField, LogField>();
            CreateMap<SerilogInfo, LogInfo>();
        }
    }
}
