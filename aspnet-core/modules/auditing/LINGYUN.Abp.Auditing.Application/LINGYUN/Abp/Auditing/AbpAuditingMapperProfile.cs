using AutoMapper;
using LINGYUN.Abp.Auditing.AuditLogs;
using LINGYUN.Abp.Auditing.Logging;
using LINGYUN.Abp.Auditing.SecurityLogs;
using LINGYUN.Abp.AuditLogging;
using LINGYUN.Abp.Logging;

namespace LINGYUN.Abp.Auditing
{
    public class AbpAuditingMapperProfile : Profile
    {
        public AbpAuditingMapperProfile()
        {
            CreateMap<AuditLogAction, AuditLogActionDto>()
                .MapExtraProperties();
            CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();
            CreateMap<EntityChange, EntityChangeDto>()
                .MapExtraProperties();
            CreateMap<AuditLog, AuditLogDto>()
                .MapExtraProperties();

            CreateMap<SecurityLog, SecurityLogDto>()
                .MapExtraProperties();

            CreateMap<LogField, LogFieldDto>();
            CreateMap<LogException, LogExceptionDto>();
            CreateMap<LogInfo, LogDto>();
        }
    }
}
