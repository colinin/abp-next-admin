using LINGYUN.Abp.AuditLogging;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.EntityChange;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EntityPropertyChangeToEntityPropertyChangeDtoMapper : MapperBase<EntityPropertyChange, EntityPropertyChangeDto>
{
    public override partial EntityPropertyChangeDto Map(EntityPropertyChange source);
    public override partial void Map(EntityPropertyChange source, EntityPropertyChangeDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class EntityChangeToEntityChangeDtoMapper : MapperBase<AuditLogging.EntityChange, EntityChangeDto>
{
    public override partial EntityChangeDto Map(AuditLogging.EntityChange source);
    public override partial void Map(AuditLogging.EntityChange source, EntityChangeDto destination);
}
