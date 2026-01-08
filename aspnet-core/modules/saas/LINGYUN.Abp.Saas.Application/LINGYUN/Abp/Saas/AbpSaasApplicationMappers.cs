using LINGYUN.Abp.Saas.Editions;
using LINGYUN.Abp.Saas.Tenants;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.Saas;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TenantConnectionStringToTenantConnectionStringDtoMapper : MapperBase<TenantConnectionString, TenantConnectionStringDto>
{
    public override partial TenantConnectionStringDto Map(TenantConnectionString source);
    public override partial void Map(TenantConnectionString source, TenantConnectionStringDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TenantToTenantDtoMapper : MapperBase<Tenant, TenantDto>
{
    [MapPropertyFromSource(nameof(TenantDto.EditionName), Use = nameof(TryGetEditionName))]
    public override partial TenantDto Map(Tenant source);

    [MapPropertyFromSource(nameof(TenantDto.EditionName), Use = nameof(TryGetEditionName))]
    public override partial void Map(Tenant source, TenantDto destination);

    private static string TryGetEditionName(Tenant source)
    {
        return source?.Edition?.DisplayName;
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class EditionToEditionDtoMapper : MapperBase<Edition, EditionDto>
{
    public override partial EditionDto Map(Edition source);
    public override partial void Map(Edition source, EditionDto destination);
}

