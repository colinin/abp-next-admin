# LINGYUN.Abp.BlobStoring.OssManagement

abp框架对象存储提供者**IBlobProvider**的Oss管理模块实现

## 配置使用

模块按需引用, 依赖于OssManagement模块, 所以需要配置远端Oss管理模块的客户端代理  

事先定义**appsettings.json**文件

```json
{
  "OssManagement": {
    "Bucket": "你定义的BucketName"
  },
  "RemoteServices": {
    "AbpOssManagement": {
      "BaseUrl": "http://127.0.0.1:30025",
      "IdentityClient": "InternalServiceClient",
      "UseCurrentAccessToken": false
    }
  },
  "IdentityClients": {
    "InternalServiceClient": {
      "Authority": "http://127.0.0.1:44385",
      "RequireHttps": false,
      "GrantType": "client_credentials",
      "Scope": "lingyun-abp-application",
      "ClientId": "InternalServiceClient",
      "ClientSecret": "1q2w3E*"
    }
  }
}

```

```csharp
[DependsOn(typeof(AbpBlobStoringOssManagementModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
