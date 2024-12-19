# LINGYUN.Abp.IdentityServer.Application

IdentityServer应用服务模块，提供IdentityServer4资源管理相关功能的应用层实现。

## 功能特性

* 客户端管理服务
  * 客户端密钥管理
  * 客户端作用域管理
  * 客户端授权类型管理
  * 客户端跨域来源管理
  * 客户端重定向URI管理
  * 客户端登出重定向URI管理
  * 客户端身份提供程序限制管理
  * 客户端声明管理
  * 客户端属性管理

* API资源管理服务
  * API资源属性管理
  * API资源密钥管理
  * API资源作用域管理
  * API资源声明管理

* API作用域管理服务
  * API作用域声明管理
  * API作用域属性管理

* 身份资源管理服务
  * 身份资源声明管理
  * 身份资源属性管理

* 持久授权管理服务

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerApplicationModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `AbpIdentityServerApplicationContractsModule` - IdentityServer应用服务契约模块
* `AbpIdentityServerDomainModule` - IdentityServer领域模块
* `AbpDddApplicationModule` - ABP DDD应用服务基础模块
* `AbpAutoMapperModule` - ABP AutoMapper对象映射模块

## 配置使用

模块实现了IdentityServer4资源的CRUD操作，主要用于管理IdentityServer4的配置资源。

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [ABP授权文档](https://docs.abp.io/en/abp/latest/Authorization)

[查看英文文档](README.EN.md)
