using AutoMapper;

namespace LINGYUN.Abp.TextTemplating;

public class AbpTextTemplatingApplicationAutoMapperProfile : Profile
{
    public AbpTextTemplatingApplicationAutoMapperProfile()
    {
        CreateMap<TextTemplateDefinition, TextTemplateDefinitionDto>()
            .ForMember(dto => dto.LayoutName, src => src.Ignore())
            .ForMember(dto => dto.FormatedDisplayName, src => src.Ignore());
    }
}
