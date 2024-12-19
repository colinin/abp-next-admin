# LINGYUN.Abp.IdentityServer.Portal

IdentityServer门户认证模块，提供企业门户的认证功能。

## 功能特性

* 门户认证
  * `PortalGrantValidator` - 门户授权验证器
    * 支持企业门户登录
    * 支持多租户认证
    * 自动切换租户
    * 企业信息验证
    * 用户密码验证
    * 安全日志记录

* 认证流程
  1. 用户使用portal发起登录请求
  2. 检查是否携带企业标识字段(EnterpriseId)
     * 未携带EnterpriseId: 返回关联了租户信息的企业列表
     * 携带EnterpriseId: 检索关联租户信息并切换到指定租户
  3. 使用password方式进行登录验证
  4. 登录成功返回token

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerPortalModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `AbpIdentityServerDomainModule` - ABP IdentityServer领域模块
* `AbpAspNetCoreMultiTenancyModule` - ABP多租户模块
* `PlatformDomainModule` - 平台领域模块

## 配置使用

### 配置门户认证

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<IIdentityServerBuilder>(builder =>
    {
        builder.AddExtensionGrantValidator<PortalGrantValidator>();
    });
}
```

### 认证请求参数

* `grant_type`: "portal" (必填)
* `enterpriseId`: 企业标识 (可选)
* `username`: 用户名 (必填)
* `password`: 密码 (必填)
* `scope`: 请求范围 (可选)

### 认证响应

* 未提供enterpriseId时:
```json
{
    "error": "invalid_grant",
    "enterprises": [
        {
            "id": "企业标识",
            "name": "企业名称",
            "code": "企业编码"
        }
    ]
}
```

* 认证成功:
```json
{
    "access_token": "访问令牌",
    "expires_in": 有效期,
    "token_type": "Bearer",
    "refresh_token": "刷新令牌"
}
```

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [ABP多租户文档](https://docs.abp.io/en/abp/latest/Multi-Tenancy)

[查看英文文档](README.EN.md)
