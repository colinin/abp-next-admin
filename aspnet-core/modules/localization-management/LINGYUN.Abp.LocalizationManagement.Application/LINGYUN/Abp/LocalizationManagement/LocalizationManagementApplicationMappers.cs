using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.LocalizationManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LanguageToLanguageDtoMapper : MapperBase<Language, LanguageDto>
{
    public override partial LanguageDto Map(Language source);
    public override partial void Map(Language source, LanguageDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ResourceToResourceDtoMapper : MapperBase<Resource, ResourceDto>
{
    public override partial ResourceDto Map(Resource source);
    public override partial void Map(Resource source, ResourceDto destination);
}
