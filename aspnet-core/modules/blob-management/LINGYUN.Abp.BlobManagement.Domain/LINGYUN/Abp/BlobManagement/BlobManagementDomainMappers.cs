using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.BlobManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class BlobContainerToBlobContainerEtoMapper : MapperBase<BlobContainer, BlobContainerEto>
{
    public override partial BlobContainerEto Map(BlobContainer source);
    public override partial void Map(BlobContainer source, BlobContainerEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class BlobToBlobEtoMapper : MapperBase<Blob, BlobEto>
{
    public override partial BlobEto Map(Blob source);
    public override partial void Map(Blob source, BlobEto destination);
}
