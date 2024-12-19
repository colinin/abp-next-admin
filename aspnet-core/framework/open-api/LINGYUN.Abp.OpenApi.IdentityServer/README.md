# LINGYUN.Abp.OpenApi.IdentityServer

OpenApi IdentityServer 集成模块，提供基于 IdentityServer 的 AppKey/AppSecret 存储实现。

## 功能特性

* 基于 IdentityServer 的 AppKey/AppSecret 存储
* 自动将 AppKey 映射为 IdentityServer 客户端标识
* 自动将 AppSecret 映射为客户端密钥
* 支持签名有效期配置
* 支持客户端名称配置
* 自动创建和更新客户端信息

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenApi.IdentityServer
```

## 模块引用

```csharp
[DependsOn(typeof(AbpOpenApiIdentityServerModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 工作原理

1. AppKey 映射
   * AppKey 将被映射为 IdentityServer 的 ClientId
   * 通过 `IClientRepository` 接口进行客户端查询和管理

2. AppSecret 映射
   * AppSecret 将被映射为客户端密钥（Client Secret）
   * 密钥将以 AppSecret 作为标识存储

3. 签名有效期
   * SignLifetime 将被存储为客户端的自定义属性
   * 属性名为 "SignLifetime"，值为有效期秒数

## 基本用法

1. 存储应用凭证
   ```csharp
   public class YourService
   {
       private readonly IAppKeyStore _appKeyStore;

       public YourService(IAppKeyStore appKeyStore)
       {
           _appKeyStore = appKeyStore;
       }

       public async Task CreateAppAsync()
       {
           var descriptor = new AppDescriptor(
               appName: "测试应用",
               appKey: "your-app-key",
               appSecret: "your-app-secret",
               signLifeTime: 300  // 5分钟有效期
           );

           await _appKeyStore.StoreAsync(descriptor);
       }
   }
   ```

2. 查询应用凭证
   ```csharp
   public class YourService
   {
       private readonly IAppKeyStore _appKeyStore;

       public YourService(IAppKeyStore appKeyStore)
       {
           _appKeyStore = appKeyStore;
       }

       public async Task<AppDescriptor> GetAppAsync(string appKey)
       {
           return await _appKeyStore.FindAsync(appKey);
       }
   }
   ```

## 更多信息

* [LINGYUN.Abp.OpenApi](../LINGYUN.Abp.OpenApi/README.md)
* [LINGYUN.Abp.OpenApi.Authorization](../LINGYUN.Abp.OpenApi.Authorization/README.md)
* [IdentityServer](https://identityserver4.readthedocs.io/)
