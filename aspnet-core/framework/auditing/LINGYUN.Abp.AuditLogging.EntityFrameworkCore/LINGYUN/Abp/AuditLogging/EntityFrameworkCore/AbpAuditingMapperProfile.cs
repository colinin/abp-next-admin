using AutoMapper;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

public class AbpAuditingMapperProfile : Profile
{
    public AbpAuditingMapperProfile()
    {
        CreateMap<Volo.Abp.AuditLogging.AuditLogAction, LINGYUN.Abp.AuditLogging.AuditLogAction>()
            .MapExtraProperties(MappingPropertyDefinitionChecks.None);
        CreateMap<Volo.Abp.AuditLogging.EntityPropertyChange, LINGYUN.Abp.AuditLogging.EntityPropertyChange>();
        CreateMap<Volo.Abp.AuditLogging.EntityChange, LINGYUN.Abp.AuditLogging.EntityChange>()
            .MapExtraProperties(MappingPropertyDefinitionChecks.None);
        CreateMap<Volo.Abp.AuditLogging.AuditLog, LINGYUN.Abp.AuditLogging.AuditLog>()
            .MapExtraProperties(MappingPropertyDefinitionChecks.None);

        CreateMap<Volo.Abp.AuditLogging.EntityChangeWithUsername, LINGYUN.Abp.AuditLogging.EntityChangeWithUsername>();

        CreateMap<Volo.Abp.Identity.IdentitySecurityLog, LINGYUN.Abp.AuditLogging.SecurityLog>(MemberList.Destination)
            .MapExtraProperties(MappingPropertyDefinitionChecks.None);
    }
}
