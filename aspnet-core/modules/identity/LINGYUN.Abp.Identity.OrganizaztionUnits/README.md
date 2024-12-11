# LINGYUN.Abp.Identity.OrganizaztionUnits

身份认证组织单元模块，提供身份认证系统与组织单元的集成功能。

## 功能特性

* 扩展AbpIdentityDomainModule模块
* 集成AbpAuthorizationOrganizationUnitsModule模块
* 支持组织单元声明动态添加
* 提供组织单元相关的身份认证功能

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(AbpAuthorizationOrganizationUnitsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

### AbpClaimsPrincipalFactoryOptions

```json
{
  "Abp": {
    "Security": {
      "Claims": {
        "DynamicClaims": {
          "OrganizationUnit": true  // 启用组织单元动态声明
        }
      }
    }
  }
}
```

## 声明类型

* `AbpOrganizationUnitClaimTypes.OrganizationUnit` - 组织单元声明类型
  * 用于标识用户所属的组织单元
  * 在用户身份验证时自动添加到声明中

## 基本用法

1. 配置组织单元声明
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpClaimsPrincipalFactoryOptions>(options =>
    {
        options.DynamicClaims.Add(AbpOrganizationUnitClaimTypes.OrganizationUnit);
    });
}
```

2. 获取用户的组织单元声明
```csharp
public class YourService
{
    private readonly ICurrentUser _currentUser;

    public YourService(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string[] GetUserOrganizationUnits()
    {
        return _currentUser.FindClaims(AbpOrganizationUnitClaimTypes.OrganizationUnit)
            .Select(c => c.Value)
            .ToArray();
    }
}
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
* [ABP组织单元文档](https://docs.abp.io/en/abp/latest/Organization-Units)
