using AutoMapper;
using LINGYUN.Abp.AuditLogging;

namespace LINGYUN.Abp.EntityChange;
public class AbpEntityChangeMapperProfile : Profile
{
    public AbpEntityChangeMapperProfile()
    {
        CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();
        CreateMap<AuditLogging.EntityChange, EntityChangeDto>()
            .MapExtraProperties();
    }
}
