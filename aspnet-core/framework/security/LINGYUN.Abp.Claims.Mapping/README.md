# LINGYUN.Abp.Claims.Mapping

引入此模块解决*IdentityServer*身份令牌映射,需要配合 *MapInboundClaims* 使用   

## 注意

身份认证服务器使用 *OpenIddict* 时无需处理  

## 配置使用


```csharp
[DependsOn(typeof(AbpClaimsMappingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
