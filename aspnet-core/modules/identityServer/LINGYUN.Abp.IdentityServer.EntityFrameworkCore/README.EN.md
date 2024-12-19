# LINGYUN.Abp.IdentityServer.EntityFrameworkCore

IdentityServer EntityFrameworkCore module, providing Entity Framework Core implementation for IdentityServer4.

## Features

* Repository Implementations
  * API Resource Repository - `EfCoreApiResourceRepository`
    * Get API Resource Names List
    * Inherits from ABP Framework's API Resource Repository Base Class

  * Identity Resource Repository - `EfCoreIdentityResourceRepository`
    * Inherits from ABP Framework's Identity Resource Repository Base Class

  * Persistent Grant Repository - `EfCorePersistentGrantRepository`
    * Inherits from ABP Framework's Persistent Grant Repository Base Class

* Database Context
  * Uses ABP Framework's `IIdentityServerDbContext`
  * Supports Multi-tenant Data Isolation

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityServerEntityFrameworkCoreModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Required Modules

* `LINGYUN.Abp.IdentityServer.AbpIdentityServerDomainModule` - IdentityServer Domain Module
* `Volo.Abp.IdentityServer.EntityFrameworkCore.AbpIdentityServerEntityFrameworkCoreModule` - ABP IdentityServer EntityFrameworkCore Module

## Configuration and Usage

### Configure Database Context

```csharp
public class YourDbContext : AbpDbContext<YourDbContext>, IIdentityServerDbContext
{
    public DbSet<ApiResource> ApiResources { get; set; }
    public DbSet<ApiScope> ApiScopes { get; set; }
    public DbSet<IdentityResource> IdentityResources { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<PersistedGrant> PersistedGrants { get; set; }
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureIdentityServer();
    }
}
```

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [ABP EntityFrameworkCore Documentation](https://docs.abp.io/en/abp/latest/Entity-Framework-Core)

[查看中文文档](README.md)
