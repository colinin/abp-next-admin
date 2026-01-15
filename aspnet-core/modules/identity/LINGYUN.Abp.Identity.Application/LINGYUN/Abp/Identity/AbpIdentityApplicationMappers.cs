using Riok.Mapperly.Abstractions;
using Volo.Abp.Identity;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.Identity;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityClaimTypeToIdentityClaimTypeDtoMapper: MapperBase<IdentityClaimType, IdentityClaimTypeDto>
{
    public override partial IdentityClaimTypeDto Map(IdentityClaimType source);
    public override partial void Map(IdentityClaimType source, IdentityClaimTypeDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityUserClaimToIdentityClaimDtoMapper : MapperBase<IdentityUserClaim, IdentityClaimDto>
{
    public override partial IdentityClaimDto Map(IdentityUserClaim source);
    public override partial void Map(IdentityUserClaim source, IdentityClaimDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityRoleClaimToIdentityClaimDtoMapper : MapperBase<IdentityRoleClaim, IdentityClaimDto>
{
    public override partial IdentityClaimDto Map(IdentityRoleClaim source);
    public override partial void Map(IdentityRoleClaim source, IdentityClaimDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityUserToIdentityUserDtoMapper : MapperBase<IdentityUser, IdentityUserDto>
{
    public override partial IdentityUserDto Map(IdentityUser source);
    public override partial void Map(IdentityUser source, IdentityUserDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentitySessionToIdentitySessionDtoMapper : MapperBase<IdentitySession, IdentitySessionDto>
{
    public override partial IdentitySessionDto Map(IdentitySession source);
    public override partial void Map(IdentitySession source, IdentitySessionDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityRoleToIdentityRoleDtoMapper : MapperBase<IdentityRole, IdentityRoleDto>
{
    public override partial IdentityRoleDto Map(IdentityRole source);
    public override partial void Map(IdentityRole source, IdentityRoleDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class OrganizationUnitToOrganizationUnitDtoMapper : MapperBase<OrganizationUnit, OrganizationUnitDto>
{
    public override partial OrganizationUnitDto Map(OrganizationUnit source);
    public override partial void Map(OrganizationUnit source, OrganizationUnitDto destination);
}
