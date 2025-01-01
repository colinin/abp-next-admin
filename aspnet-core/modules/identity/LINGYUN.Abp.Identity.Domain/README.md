# LINGYUN.Abp.Identity.Domain

身份认证领域模块，提供身份认证相关的核心功能实现。

## 功能特性

* 扩展Volo.Abp.Identity.AbpIdentityDomainModule模块
* 提供身份会话管理功能
* 提供身份会话清理功能
* 支持分布式事件

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(Volo.Abp.Identity.AbpIdentityDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

### IdentitySessionCleanupOptions

```json
{
  "Identity": {
    "Session": {
      "Cleanup": {
        "IsEnabled": false,           // 是否启用会话清理，默认：false
        "CleanupPeriod": 3600000,    // 会话清理间隔（毫秒），默认：1小时
        "InactiveTimeSpan": "30.00:00:00" // 不活跃会话保持时长，默认：30天
      }
    }
  }
}
```

### IdentitySessionSignInOptions

```json
{
  "Identity": {
    "Session": {
      "SignIn": {
        "AuthenticationSchemes": ["Identity.Application"], // 用于处理的身份认证方案
        "SignInSessionEnabled": false,     // 是否启用SignInManager登录会话，默认：false
        "SignOutSessionEnabled": false     // 是否启用SignInManager登出会话，默认：false
      }
    }
  }
}
```

## 基本用法

1. 配置身份会话管理
```csharp
Configure<IdentitySessionSignInOptions>(options =>
{
    options.SignInSessionEnabled = true;    // 启用登录会话
    options.SignOutSessionEnabled = true;   // 启用登出会话
});
```

2. 配置会话清理
```csharp
Configure<IdentitySessionCleanupOptions>(options =>
{
    options.IsCleanupEnabled = true;    // 启用会话清理
    options.CleanupPeriod = 3600000;    // 设置清理间隔为1小时
    options.InactiveTimeSpan = TimeSpan.FromDays(7);  // 设置不活跃会话保持时间为7天
});
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
