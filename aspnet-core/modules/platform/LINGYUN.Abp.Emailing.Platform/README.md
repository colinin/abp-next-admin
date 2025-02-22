# LINGYUN.Abp.Emailing.Platform

abp框架邮件发送接口**IEmailSender**的消息处理平台实现  

通过平台服务接口实现邮件消息管理,发送,查询等功能  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpEmailingPlatformModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
## 配置项说明

```json

{
  "RemoteServices": {
    "Platform": {
      "BaseUrl": "http://127.0.0.1:30025",
      "IdentityClient": "InternalServiceClient",
      "UseCurrentAccessToken": "False"
    }
  },
  "IdentityClients": {
    "InternalServiceClient": {
      "Authority": "http://127.0.0.1:44385",
      "ClientId": "InternalServiceClient",
      "ClientSecret": "1q2w3e*",
      "GrantType": "client_credentials",
      "RequireHttps": "False",
      "Scope": "lingyun-abp-application"
    }
  },
}

```

## 相关链接

* [平台服务模块](../README.md)
* [邮件发送集成](https://abp.io/docs/latest/framework/infrastructure/emailing)
* [动态接口代理](https://abp.io/docs/latest/framework/api-development/dynamic-csharp-clients)

## 其他

