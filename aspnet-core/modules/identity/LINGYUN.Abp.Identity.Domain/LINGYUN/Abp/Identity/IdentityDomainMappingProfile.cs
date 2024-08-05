using AutoMapper;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

public class IdentityDomainMappingProfile : Profile
{
    public IdentityDomainMappingProfile()
    {
        CreateMap<IdentitySession, IdentitySessionEto>();
    }
}
