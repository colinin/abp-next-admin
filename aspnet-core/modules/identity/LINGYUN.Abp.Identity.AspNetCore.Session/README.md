# LINGYUN.Abp.Identity.AspNetCore.Session

用户会话登录扩展模块,主要处理 *AspNetCore.Identity* 提供的登录/登出事件来管理会话   

出于模块职责分离原则, 请勿与 *LINGYUN.Abp.Identity.Session.AspNetCore* 模块混淆  

## 配置使用

```csharp
[DependsOn(typeof(AbpIdentityAspNetCoreSessionModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 功能特性

* 扩展AbpIdentityAspNetCoreModule模块
* 提供AspNetCore环境下的会话管理功能
* 自定义身份认证服务实现
* 集成AspNetCore.Identity的登录/登出事件

## 服务实现

* AbpIdentitySessionAuthenticationService - 自定义身份认证服务
  * 处理用户登录/登出事件
  * 管理用户会话状态
  * 与Identity会话系统集成

## 基本用法

1. 配置身份认证服务
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<IAuthenticationService, AbpIdentitySessionAuthenticationService>();
}
```

2. 使用身份认证服务
```csharp
public class YourService
{
    private readonly IAuthenticationService _authenticationService;

    public YourService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
    {
        await _authenticationService.SignInAsync(context, scheme, principal, properties);
    }

    public async Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        await _authenticationService.SignOutAsync(context, scheme, properties);
    }
}
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
* [ASP.NET Core Identity文档](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)
