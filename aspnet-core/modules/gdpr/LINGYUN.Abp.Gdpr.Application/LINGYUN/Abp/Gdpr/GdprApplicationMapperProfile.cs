using AutoMapper;

namespace LINGYUN.Abp.Gdpr;
public class GdprApplicationMapperProfile : Profile
{
    public GdprApplicationMapperProfile()
    {
        CreateMap<GdprRequest, GdprRequestDto>();
    }
}
