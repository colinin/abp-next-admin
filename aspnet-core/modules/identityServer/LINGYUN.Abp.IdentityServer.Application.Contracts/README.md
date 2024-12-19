# LINGYUN.Abp.IdentityServer.Application.Contracts

IdentityServer应用服务契约模块，定义IdentityServer4资源管理相关功能的应用服务接口和DTO。

## 功能特性

* 权限定义
  * 客户端权限
    * 默认权限 - `AbpIdentityServer.Clients`
    * 创建权限 - `AbpIdentityServer.Clients.Create`
    * 更新权限 - `AbpIdentityServer.Clients.Update`
    * 删除权限 - `AbpIdentityServer.Clients.Delete`
    * 克隆权限 - `AbpIdentityServer.Clients.Clone`
    * 管理权限 - `AbpIdentityServer.Clients.ManagePermissions`
    * 管理声明 - `AbpIdentityServer.Clients.ManageClaims`
    * 管理密钥 - `AbpIdentityServer.Clients.ManageSecrets`
    * 管理属性 - `AbpIdentityServer.Clients.ManageProperties`

  * API资源权限
    * 默认权限 - `AbpIdentityServer.ApiResources`
    * 创建权限 - `AbpIdentityServer.ApiResources.Create`
    * 更新权限 - `AbpIdentityServer.ApiResources.Update`
    * 删除权限 - `AbpIdentityServer.ApiResources.Delete`
    * 管理声明 - `AbpIdentityServer.ApiResources.ManageClaims`
    * 管理密钥 - `AbpIdentityServer.ApiResources.ManageSecrets`
    * 管理作用域 - `AbpIdentityServer.ApiResources.ManageScopes`
    * 管理属性 - `AbpIdentityServer.ApiResources.ManageProperties`

  * API作用域权限
    * 默认权限 - `AbpIdentityServer.ApiScopes`
    * 创建权限 - `AbpIdentityServer.ApiScopes.Create`
    * 更新权限 - `AbpIdentityServer.ApiScopes.Update`
    * 删除权限 - `AbpIdentityServer.ApiScopes.Delete`
    * 管理声明 - `AbpIdentityServer.ApiScopes.ManageClaims`
    * 管理属性 - `AbpIdentityServer.ApiScopes.ManageProperties`

  * 身份资源权限
    * 默认权限 - `AbpIdentityServer.IdentityResources`
    * 创建权限 - `AbpIdentityServer.IdentityResources.Create`
    * 更新权限 - `AbpIdentityServer.IdentityResources.Update`
    * 删除权限 - `AbpIdentityServer.IdentityResources.Delete`
    * 管理声明 - `AbpIdentityServer.IdentityResources.ManageClaims`
    * 管理属性 - `AbpIdentityServer.IdentityResources.ManageProperties`

  * 授权许可权限
    * 默认权限 - `AbpIdentityServer.Grants`
    * 删除权限 - `AbpIdentityServer.Grants.Delete`

* 本地化资源
  * 支持多语言本地化
  * 内置中英文资源

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerApplicationContractsModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `AbpAuthorizationModule` - ABP授权模块
* `AbpDddApplicationContractsModule` - ABP DDD应用服务契约模块
* `AbpIdentityServerDomainSharedModule` - IdentityServer领域共享模块

## 配置使用

模块提供了IdentityServer4资源管理所需的应用服务接口定义和数据传输对象。所有权限默认只对宿主租户开放。

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [ABP授权文档](https://docs.abp.io/en/abp/latest/Authorization)

[查看英文文档](README.EN.md)
