# LINGYUN.Abp.Identity.Domain.Shared

身份认证领域共享模块，提供身份认证相关的基础定义。

## 功能特性

* 提供身份认证相关的本地化资源
* 提供身份认证相关的虚拟文件系统配置
* 扩展Volo.Abp.Identity.AbpIdentityDomainSharedModule模块

## 模块引用

```csharp
[DependsOn(typeof(AbpIdentityDomainSharedModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

无需额外配置。

## 基本用法

1. 引用模块
```csharp
[DependsOn(typeof(AbpIdentityDomainSharedModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 使用本地化资源
```csharp
public class YourClass
{
    private readonly IStringLocalizer<IdentityResource> _localizer;

    public YourClass(IStringLocalizer<IdentityResource> localizer)
    {
        _localizer = localizer;
    }

    public string GetLocalizedString(string key)
    {
        return _localizer[key];
    }
}
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
