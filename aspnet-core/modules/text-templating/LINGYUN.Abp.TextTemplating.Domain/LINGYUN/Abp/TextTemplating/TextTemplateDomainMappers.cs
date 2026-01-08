using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.TextTemplating;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TextTemplateToTextTemplateEtoMapper : MapperBase<TextTemplate, TextTemplateEto>
{
    public override partial TextTemplateEto Map(TextTemplate source);
    public override partial void Map(TextTemplate source, TextTemplateEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TextTemplateDefinitionToTextTemplateDefinitionEtoMapper : MapperBase<TextTemplateDefinition, TextTemplateDefinitionEto>
{
    public override partial TextTemplateDefinitionEto Map(TextTemplateDefinition source);
    public override partial void Map(TextTemplateDefinition source, TextTemplateDefinitionEto destination);
}
