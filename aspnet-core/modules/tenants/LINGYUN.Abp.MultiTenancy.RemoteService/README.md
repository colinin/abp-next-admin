# LINGYUN.Abp.MultiTenancy.RemoteService

abp 多租户远程服务组件,引用此模块将首先从分布式缓存查询当前租户配置

如果缓存不存在,则调用远程租户服务接口获取租户数据,并存储到分布式缓存中

## 配置使用

模块按需引用，因为远程服务接口定义了授权策略，需要配置接口客户端授权

具体**RemoteServices**与**IdentityClients**配置请阅读abp文档

[RemoteServices配置参阅](https://docs.abp.io/en/abp/latest/API/Dynamic-CSharp-API-Clients)

[IdentityClients配置参阅](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.IdentityModel/Volo/Abp/IdentityModel/IdentityClientConfiguration.cs)

事先定义**appsettings.json**文件

```json
{
  "RemoteServices": {
    "TenantManagement": {
      "BaseUrl": "http://localhost:30000/",
      "IdentityClient": "tenant-finder-client"
    }
  },
  "IdentityClients": {
    "tenant-finder-client": {
      "Authority": "http://localhost:44385",
      "RequireHttps": false,
      "GrantType": "client_credentials",
      "ClientId": "tenant-finder-client",
      "Scope": "tenant-service",
      "ClientSecret": "1q2w3e*"
    }
  }
}

```

```csharp
[DependsOn(typeof(AbpRemoteServiceMultiTenancyModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```