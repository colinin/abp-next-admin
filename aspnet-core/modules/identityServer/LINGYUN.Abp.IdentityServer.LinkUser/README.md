# LINGYUN.Abp.IdentityServer.LinkUser

IdentityServer用户关联模块，提供用户关联的扩展授权类型支持。

## 功能特性

* 扩展授权验证器
  * `LinkUserGrantValidator` - 用户关联授权验证器
    * 授权类型：`link_user`
    * 支持验证访问令牌
    * 支持验证用户关联关系
    * 支持多租户场景
    * 支持自定义声明扩展

* 本地化支持
  * 内置中英文资源
  * 支持扩展其他语言

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerLinkUserModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `AbpIdentityServerDomainModule` - ABP IdentityServer领域模块

## 配置使用

### 授权请求参数

* `grant_type` - 必须为 `link_user`
* `access_token` - 当前用户的访问令牌
* `LinkUserId` - 要关联的用户ID
* `LinkTenantId` - 要关联的用户所属租户ID（可选）

### 授权请求示例

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=link_user&
access_token=current_user_access_token&
LinkUserId=target_user_id&
LinkTenantId=target_tenant_id
```

### 自定义声明扩展

```csharp
public class CustomLinkUserGrantValidator : LinkUserGrantValidator
{
    protected override Task AddCustomClaimsAsync(List<Claim> customClaims, IdentityUser user, ExtensionGrantValidationContext context)
    {
        // 添加自定义声明
        customClaims.Add(new Claim("custom_claim", "custom_value"));

        return base.AddCustomClaimsAsync(customClaims, user, context);
    }
}
```

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Authentication)

[查看英文文档](README.EN.md)
