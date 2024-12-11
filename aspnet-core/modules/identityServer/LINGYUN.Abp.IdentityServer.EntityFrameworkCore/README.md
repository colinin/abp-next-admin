# LINGYUN.Abp.IdentityServer.EntityFrameworkCore

IdentityServer EntityFrameworkCore模块，提供IdentityServer4的Entity Framework Core实现。

## 功能特性

* 仓储实现
  * API资源仓储 - `EfCoreApiResourceRepository`
    * 获取API资源名称列表
    * 继承自ABP框架的API资源仓储基类

  * 身份资源仓储 - `EfCoreIdentityResourceRepository`
    * 继承自ABP框架的身份资源仓储基类

  * 持久授权仓储 - `EfCorePersistentGrantRepository`
    * 继承自ABP框架的持久授权仓储基类

* 数据库上下文
  * 使用ABP框架的`IIdentityServerDbContext`
  * 支持多租户数据隔离

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerEntityFrameworkCoreModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `LINGYUN.Abp.IdentityServer.AbpIdentityServerDomainModule` - IdentityServer领域模块
* `Volo.Abp.IdentityServer.EntityFrameworkCore.AbpIdentityServerEntityFrameworkCoreModule` - ABP IdentityServer EntityFrameworkCore模块

## 配置使用

### 配置数据库上下文

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

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [ABP EntityFrameworkCore文档](https://docs.abp.io/en/abp/latest/Entity-Framework-Core)

[查看英文文档](README.EN.md)
