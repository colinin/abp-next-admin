using LINGYUN.Abp.MultiTenancy.Editions;
using LINGYUN.Abp.Saas.Editions;
using LINGYUN.Abp.Saas.Tenants;
using Riok.Mapperly.Abstractions;
using System;
using Volo.Abp.Data;
using Volo.Abp.Mapperly;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Saas;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EditionToEditionInfoMapper : MapperBase<Edition, EditionInfo>
{
    public override partial EditionInfo Map(Edition source);
    public override partial void Map(Edition source, EditionInfo destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EditionToEditionEtoMapper : MapperBase<Edition, EditionEto>
{
    public override partial EditionEto Map(Edition source);
    public override partial void Map(Edition source, EditionEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TenantToTenantConfigurationMapper : MapperBase<Tenant, TenantConfiguration>
{
    [MapPropertyFromSource(nameof(TenantConfiguration.ConnectionStrings), Use = nameof(TryGetConnectionStrings))]
    [MapPropertyFromSource(nameof(TenantConfiguration.IsActive), Use = nameof(TryGetIsActive))]
    public override partial TenantConfiguration Map(Tenant source);

    [MapPropertyFromSource(nameof(TenantConfiguration.ConnectionStrings), Use = nameof(TryGetConnectionStrings))]
    [MapPropertyFromSource(nameof(TenantConfiguration.IsActive), Use = nameof(TryGetIsActive))]
    public override partial void Map(Tenant source, TenantConfiguration destination);

    [UserMapping]
    private static ConnectionStrings TryGetConnectionStrings(Tenant source)
    {
        var connStrings = new ConnectionStrings();

        foreach (var connectionString in source.ConnectionStrings)
        {
            connStrings[connectionString.Name] = connectionString.Value;
        }

        return connStrings;
    }

    [UserMapping]
    private static bool TryGetIsActive(Tenant source)
    {
        if (!source.IsActive)
        {
            return false;
        }
        // Injection IClock ?
        if (source.EnableTime.HasValue && DateTime.Now < source.EnableTime)
        {
            return false;
        }

        if (source.DisableTime.HasValue && DateTime.Now > source.DisableTime)
        {
            return false;
        }

        return true;
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TenantToTenantEtoMapper : MapperBase<Tenant, TenantEto>
{
    public override partial TenantEto Map(Tenant source);
    public override partial void Map(Tenant source, TenantEto destination);
}
