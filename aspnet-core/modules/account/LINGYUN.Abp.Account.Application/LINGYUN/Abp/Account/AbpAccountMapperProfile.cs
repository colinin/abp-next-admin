using AutoMapper;
using LINGYUN.Abp.AuditLogging;

namespace LINGYUN.Abp.Account;

public class AbpAccountMapperProfile : Profile
{
    public AbpAccountMapperProfile()
    {
        CreateMap<SecurityLog, SecurityLogDto>(MemberList.Destination);
    }
}
