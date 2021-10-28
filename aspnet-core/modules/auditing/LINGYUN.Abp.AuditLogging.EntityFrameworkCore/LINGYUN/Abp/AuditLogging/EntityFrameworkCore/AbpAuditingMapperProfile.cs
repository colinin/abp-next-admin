using AutoMapper;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore
{
    public class AbpAuditingMapperProfile : Profile
    {
        public AbpAuditingMapperProfile()
        {
            CreateMap<Volo.Abp.AuditLogging.AuditLogAction, LINGYUN.Abp.AuditLogging.AuditLogAction>()
                .MapExtraProperties();
            CreateMap<Volo.Abp.AuditLogging.EntityPropertyChange, LINGYUN.Abp.AuditLogging.EntityPropertyChange>();
            CreateMap<Volo.Abp.AuditLogging.EntityChange, LINGYUN.Abp.AuditLogging.EntityChange>()
                .MapExtraProperties();
            CreateMap<Volo.Abp.AuditLogging.AuditLog, LINGYUN.Abp.AuditLogging.AuditLog>()
                .MapExtraProperties();

            CreateMap<Volo.Abp.Identity.IdentitySecurityLog, LINGYUN.Abp.AuditLogging.SecurityLog>()
                .MapExtraProperties();
        }
    }
}
