# LINGYUN.Abp.IdentityModel

abp框架 **IIdentityModelAuthenticationService**接口缓存版本

因官方**Volo.Abp.IdentityModel**模块没有对接口授权缓存,每次内部调用都会请求IDS服务器,会加重IDS服务器压力,
创建使用缓存的接口实现

## 配置使用

模块按需引用，需要配置**Volo.Abp.IdentityModel**模块所需配置项


```csharp
[DependsOn(typeof(AbpCachedIdentityModelModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```