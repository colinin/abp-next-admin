using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.DataProtectionManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EntityEnumInfoToEntityEnumInfoDtoMapper : MapperBase<EntityEnumInfo, EntityEnumInfoDto>
{
    public override partial EntityEnumInfoDto Map(EntityEnumInfo source);
    public override partial void Map(EntityEnumInfo source, EntityEnumInfoDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EntityPropertyInfoToEntityPropertyInfoDtoMapper : MapperBase<EntityPropertyInfo, EntityPropertyInfoDto>
{
    public override partial EntityPropertyInfoDto Map(EntityPropertyInfo source);
    public override partial void Map(EntityPropertyInfo source, EntityPropertyInfoDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EntityTypeInfoToEntityTypeInfoDtoMapper : MapperBase<EntityTypeInfo, EntityTypeInfoDto>
{
    public override partial EntityTypeInfoDto Map(EntityTypeInfo source);
    public override partial void Map(EntityTypeInfo source, EntityTypeInfoDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class RoleEntityRuleToRoleEntityRuleDtoMapper : MapperBase<RoleEntityRule, RoleEntityRuleDto>
{
    [MapProperty(nameof(RoleEntityRule.AccessedProperties), nameof(RoleEntityRuleDto.AccessedProperties), Use = nameof(MapToArray))]
    public override partial RoleEntityRuleDto Map(RoleEntityRule source);

    [MapProperty(nameof(RoleEntityRule.AccessedProperties), nameof(RoleEntityRuleDto.AccessedProperties), Use = nameof(MapToArray))]
    public override partial void Map(RoleEntityRule source, RoleEntityRuleDto destination);

    [UserMapping]
    private string[] MapToArray(string val)
    {
        if (val.IsNullOrWhiteSpace())
        {
            return Array.Empty<string>();
        }
        return val.Split(',').ToArray();
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrganizationUnitEntityRuleToOrganizationUnitEntityRuleDtoMapper : MapperBase<OrganizationUnitEntityRule, OrganizationUnitEntityRuleDto>
{
    [MapProperty(nameof(OrganizationUnitEntityRule.AccessedProperties), nameof(OrganizationUnitEntityRuleDto.AccessedProperties), Use = nameof(MapToArray))]
    public override partial OrganizationUnitEntityRuleDto Map(OrganizationUnitEntityRule source);

    [MapProperty(nameof(OrganizationUnitEntityRule.AccessedProperties), nameof(OrganizationUnitEntityRuleDto.AccessedProperties), Use = nameof(MapToArray))]
    public override partial void Map(OrganizationUnitEntityRule source, OrganizationUnitEntityRuleDto destination);

    [UserMapping]
    private string[] MapToArray(string val)
    {
        if (val.IsNullOrWhiteSpace())
        {
            return Array.Empty<string>();
        }
        return val.Split(',').ToArray();
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SubjectStrategyToSubjectStrategyDtoMapper : MapperBase<SubjectStrategy, SubjectStrategyDto>
{
    public override partial SubjectStrategyDto Map(SubjectStrategy source);
    public override partial void Map(SubjectStrategy source, SubjectStrategyDto destination);
}
