# LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore

本地化管理模块的Entity Framework Core集成实现，提供数据访问和持久化功能。

## 功能特性

* 实现本地化管理的数据库映射
* 支持自定义表前缀和Schema
* 提供仓储接口的EF Core实现
* 支持语言、资源、文本的数据库操作

## 模块引用

```csharp
[DependsOn(typeof(AbpLocalizationManagementEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAbpDbContext<YourDbContext>(options =>
    {
        options.AddRepository<Language, EfCoreLanguageRepository>();
        options.AddRepository<Resource, EfCoreResourceRepository>();
        options.AddRepository<Text, EfCoreTextRepository>();
    });

    Configure<AbpDbContextOptions>(options =>
    {
        options.UseMySQL(); // 或其他数据库
    });
}
```

数据库表配置选项：
```csharp
public class LocalizationModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
{
    public LocalizationModelBuilderConfigurationOptions(
       string tablePrefix = "",
       string schema = null)
       : base(tablePrefix, schema)
    {
    }
}
```

## 数据库表

* Languages - 语言表
* Resources - 资源表
* Texts - 文本表

## 更多信息

* [English documentation](./README.EN.md)
