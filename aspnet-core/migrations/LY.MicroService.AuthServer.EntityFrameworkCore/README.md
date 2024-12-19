# LY.MicroService.AuthServer.EntityFrameworkCore

认证服务器数据库迁移模块，提供认证服务器所需的数据库迁移功能。

[English](./README.EN.md)

## 功能特性

* 集成OpenIddict认证框架的数据库迁移
* 集成ABP Identity模块的数据库迁移
* 集成ABP权限管理模块的数据库迁移
* 集成ABP设置管理模块的数据库迁移
* 集成ABP特性管理模块的数据库迁移
* 集成ABP SaaS多租户模块的数据库迁移
* 集成文本模板模块的数据库迁移
* 支持MySQL数据库

## 模块引用

```csharp
[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule)
)]
public class YourModule : AbpModule
{
    // other
}
```

## 配置项

```json
{
  "ConnectionStrings": {
    "AuthServerDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置AuthServerDbMigrator连接字符串

2. 添加数据库上下文
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       context.Services.AddAbpDbContext<AuthServerMigrationsDbContext>();

       Configure<AbpDbContextOptions>(options =>
       {
           options.UseMySQL();
       });
   }
   ```

3. 执行数据库迁移
   * 使用EF Core命令行工具执行迁移
   ```bash
   dotnet ef database update
   ```

## 注意事项

* 确保数据库连接字符串中包含正确的权限，以便能够创建和修改数据库表
* 在执行迁移之前，建议先备份数据库
* 迁移脚本会自动处理模块间的依赖关系
