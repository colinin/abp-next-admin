using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.DataProtectionManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EntityTypeInfoToEntityTypeInfoEtoMapper : MapperBase<EntityTypeInfo, EntityTypeInfoEto>
{
    public override partial EntityTypeInfoEto Map(EntityTypeInfo source);
    public override partial void Map(EntityTypeInfo source, EntityTypeInfoEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class RoleEntityRuleToRoleEntityRuleEtoMapper : MapperBase<RoleEntityRule, RoleEntityRuleEto>
{
    public override partial RoleEntityRuleEto Map(RoleEntityRule source);
    public override partial void Map(RoleEntityRule source, RoleEntityRuleEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrganizationUnitEntityRuleToOrganizationUnitEntityRuleEtoMapper : MapperBase<OrganizationUnitEntityRule, OrganizationUnitEntityRuleEto>
{
    public override partial OrganizationUnitEntityRuleEto Map(OrganizationUnitEntityRule source);
    public override partial void Map(OrganizationUnitEntityRule source, OrganizationUnitEntityRuleEto destination);
}
