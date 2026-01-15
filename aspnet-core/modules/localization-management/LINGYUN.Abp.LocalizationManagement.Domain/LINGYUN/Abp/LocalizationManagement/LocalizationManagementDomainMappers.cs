using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.LocalizationManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TextToTextEtoMapper : MapperBase<Text, TextEto>
{
    public override partial TextEto Map(Text source);
    public override partial void Map(Text source, TextEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ResourceToResourceEtoMapper : MapperBase<Resource, ResourceEto>
{
    public override partial ResourceEto Map(Resource source);
    public override partial void Map(Resource source, ResourceEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LanguageToLanguageEtoMapper : MapperBase<Language, LanguageEto>
{
    public override partial LanguageEto Map(Language source);
    public override partial void Map(Language source, LanguageEto destination);
}
