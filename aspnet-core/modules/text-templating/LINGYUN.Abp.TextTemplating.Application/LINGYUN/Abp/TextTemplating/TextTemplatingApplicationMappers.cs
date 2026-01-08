using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.TextTemplating;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TextTemplateDefinitionToTextTemplateDefinitionDtoMapper : MapperBase<TextTemplateDefinition, TextTemplateDefinitionDto>
{
    public override partial TextTemplateDefinitionDto Map(TextTemplateDefinition source);
    public override partial void Map(TextTemplateDefinition source, TextTemplateDefinitionDto destination);
}
