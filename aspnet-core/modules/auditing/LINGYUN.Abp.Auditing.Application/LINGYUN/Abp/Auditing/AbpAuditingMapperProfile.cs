using AutoMapper;
using LINGYUN.Abp.Auditing.AuditLogs;
using LINGYUN.Abp.Auditing.Logging;
using LINGYUN.Abp.Auditing.SecurityLogs;
using LINGYUN.Abp.AuditLogging;
using LINGYUN.Abp.Logging;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.Auditing;

public class AbpAuditingMapperProfile : Profile
{
    public AbpAuditingMapperProfile()
    {
        CreateMap<AuditLogAction, AuditLogActionDto>()
            .MapExtraProperties(MappingPropertyDefinitionChecks.None);
        CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();
        CreateMap<EntityChangeWithUsername, EntityChangeWithUsernameDto>();
        CreateMap<EntityChange, EntityChangeDto>()
            .MapExtraProperties(MappingPropertyDefinitionChecks.None);
        CreateMap<AuditLog, AuditLogDto>()
            .MapExtraProperties(MappingPropertyDefinitionChecks.None);

        CreateMap<SecurityLog, SecurityLogDto>(MemberList.Destination)
            .MapExtraProperties(MappingPropertyDefinitionChecks.None);

        CreateMap<LogField, LogFieldDto>();
        CreateMap<LogException, LogExceptionDto>();
        CreateMap<LogInfo, LogDto>();
    }
}
