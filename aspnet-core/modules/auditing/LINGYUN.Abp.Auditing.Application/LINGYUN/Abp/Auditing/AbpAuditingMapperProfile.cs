using AutoMapper;
using LINGYUN.Abp.Auditing.Logging;
using LINGYUN.Abp.Auditing.Security;
using Volo.Abp.AuditLogging;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

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

            CreateMap<IdentitySecurityLog, SecurityLogDto>();
        }
    }
}
