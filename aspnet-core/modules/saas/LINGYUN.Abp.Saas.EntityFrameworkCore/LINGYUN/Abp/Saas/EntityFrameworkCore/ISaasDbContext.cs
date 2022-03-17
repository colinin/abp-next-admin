using LINGYUN.Abp.Saas.Editions;
using LINGYUN.Abp.Saas.Tenants;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Saas.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpSaasDbProperties.ConnectionStringName)]
public interface ISaasDbContext : IEfCoreDbContext
{
    DbSet<Edition> Editions { get; }
    DbSet<Tenant> Tenants { get; }
    DbSet<TenantConnectionString> TenantConnectionStrings { get; }
}
