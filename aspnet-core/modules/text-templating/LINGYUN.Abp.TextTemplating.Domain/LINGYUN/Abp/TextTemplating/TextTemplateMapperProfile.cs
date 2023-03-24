using AutoMapper;

namespace LINGYUN.Abp.TextTemplating;
public class TextTemplateMapperProfile : Profile
{
    public TextTemplateMapperProfile()
    {
        CreateMap<TextTemplate, TextTemplateEto>();
        CreateMap<TextTemplateDefinition, TextTemplateDefinitionEto>();
    }
}
