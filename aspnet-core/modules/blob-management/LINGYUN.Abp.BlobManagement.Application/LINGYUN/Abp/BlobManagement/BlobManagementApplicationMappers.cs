using LINGYUN.Abp.BlobManagement.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.BlobManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class BlobContainerToBlobContainerDtoMapper : MapperBase<BlobContainer, BlobContainerDto>
{
    public override partial BlobContainerDto Map(BlobContainer source);
    public override partial void Map(BlobContainer source, BlobContainerDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class BlobToBlobDtoMapper : MapperBase<Blob, BlobDto>
{
    public override partial BlobDto Map(Blob source);
    public override partial void Map(Blob source, BlobDto destination);
}
