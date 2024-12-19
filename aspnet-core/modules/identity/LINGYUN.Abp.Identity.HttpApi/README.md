# LINGYUN.Abp.Identity.HttpApi

身份认证HTTP API模块，提供身份认证相关的HTTP API接口。

## 功能特性

* 扩展Volo.Abp.Identity.AbpIdentityHttpApiModule模块
* 提供身份认证相关的HTTP API接口
* 支持本地化资源的MVC数据注解
* 自动注册MVC应用程序部件

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API控制器

* IdentityUserController - 用户管理API
* IdentityRoleController - 角色管理API
* IdentityClaimTypeController - 声明类型管理API
* IdentitySecurityLogController - 安全日志API
* IdentitySettingsController - 身份认证设置API
* ProfileController - 用户配置文件API

## API路由

* `/api/identity/users` - 用户管理API路由
* `/api/identity/roles` - 角色管理API路由
* `/api/identity/claim-types` - 声明类型管理API路由
* `/api/identity/security-logs` - 安全日志API路由
* `/api/identity/settings` - 身份认证设置API路由
* `/api/identity/my-profile` - 用户配置文件API路由

## 基本用法

1. 配置本地化
```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
    {
        options.AddAssemblyResource(typeof(IdentityResource), typeof(AbpIdentityApplicationContractsModule).Assembly);
    });
}
```

2. 使用API
```csharp
// 获取用户列表
GET /api/identity/users

// 获取指定用户
GET /api/identity/users/{id}

// 创建用户
POST /api/identity/users
{
    "userName": "admin",
    "email": "admin@abp.io",
    "password": "1q2w3E*"
}

// 更新用户
PUT /api/identity/users/{id}
{
    "userName": "admin",
    "email": "admin@abp.io"
}

// 删除用户
DELETE /api/identity/users/{id}
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
* [ABP HTTP API文档](https://docs.abp.io/en/abp/latest/API/HTTP-API)
