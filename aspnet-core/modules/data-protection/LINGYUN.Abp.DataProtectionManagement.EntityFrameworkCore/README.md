# LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore

数据权限管理EntityFrameworkCore模块，提供数据权限管理的数据访问实现。

## 功能

* 数据权限管理仓储实现
* 数据库映射配置
* 数据库迁移

## 仓储实现

* `EfCoreDataProtectionRepository` - 数据权限仓储EF Core实现
  * 实现 `IDataProtectionRepository` 接口
  * 提供数据权限的CRUD操作
  * 支持数据过滤

## 数据库映射配置

```csharp
public static class DataProtectionDbContextModelCreatingExtensions
{
    public static void ConfigureDataProtectionManagement(
        this ModelBuilder builder,
        Action<DataProtectionModelBuilderConfigurationOptions> optionsAction = null)
    {
        builder.Entity<DataProtection>(b =>
        {
            b.ToTable(options.TablePrefix + "DataProtections", options.Schema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(DataProtectionConsts.MaxNameLength);
            b.Property(x => x.DisplayName).HasMaxLength(DataProtectionConsts.MaxDisplayNameLength);
            b.Property(x => x.Description).HasMaxLength(DataProtectionConsts.MaxDescriptionLength);

            b.HasIndex(x => x.Name);
        });
    }
}
```

## 配置使用

1. 添加模块依赖

```csharp
[DependsOn(typeof(AbpDataProtectionManagementEntityFrameworkCoreModule))]
public class YourModule : AbpModule
{
}
```

2. 配置DbContext

```csharp
public class YourDbContext : AbpDbContext<YourDbContext>, IDataProtectionManagementDbContext
{
    public DbSet<DataProtection> DataProtections { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureDataProtectionManagement();
    }
}
```

## 相关链接

* [English document](./README.EN.md)
