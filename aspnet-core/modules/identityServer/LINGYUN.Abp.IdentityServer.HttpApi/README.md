# LINGYUN.Abp.IdentityServer.HttpApi

IdentityServer HTTP API模块，提供IdentityServer4资源管理的HTTP API接口。

## 功能特性

* API控制器
  * API作用域控制器 - `ApiScopeController`
    * 创建API作用域 - POST `/api/identity-server/api-scopes`
    * 删除API作用域 - DELETE `/api/identity-server/api-scopes/{id}`
    * 获取API作用域 - GET `/api/identity-server/api-scopes/{id}`
    * 获取API作用域列表 - GET `/api/identity-server/api-scopes`
    * 更新API作用域 - PUT `/api/identity-server/api-scopes/{id}`

  * API资源控制器 - `ApiResourceController`
    * 提供API资源的CRUD操作接口
    * 路由前缀：`/api/identity-server/api-resources`

* 本地化支持
  * 继承ABP UI资源的本地化配置
  * 支持多语言

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerHttpApiModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `AbpIdentityServerApplicationContractsModule` - IdentityServer应用服务契约模块
* `AbpAspNetCoreMvcModule` - ABP ASP.NET Core MVC模块

## 配置使用

### 配置远程服务名称

```csharp
[RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
[Area("identity-server")]
[Route("api/identity-server/[controller]")]
public class YourController : AbpControllerBase
{
    // ...
}
```

### 添加本地化资源

```csharp
Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Get<AbpIdentityServerResource>()
        .AddBaseTypes(typeof(AbpUiResource));
});
```

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [ABP ASP.NET Core MVC文档](https://docs.abp.io/en/abp/latest/AspNetCore-MVC)

[查看英文文档](README.EN.md)
