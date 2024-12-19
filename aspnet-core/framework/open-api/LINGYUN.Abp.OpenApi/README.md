# LINGYUN.Abp.OpenApi

OpenApi 认证模块，为 ABP 应用程序提供基于 AppKey/AppSecret 的 API 签名认证功能。

## 功能特性

* 支持 AppKey/AppSecret 认证
* 支持请求签名验证
* 支持防重放攻击（Nonce随机数验证）
* 支持请求时间戳验证
* 支持客户端白名单
* 支持IP地址白名单
* 支持多语言错误消息

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenApi
```

## 模块引用

```csharp
[DependsOn(typeof(AbpOpenApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "OpenApi": {
    "IsEnabled": true,                   // 是否启用API签名检查，默认: true
    "RequestNonceExpireIn": "00:10:00",  // 请求随机数过期时间，默认: 10分钟
    "AppDescriptors": [                  // AppKey配置列表
      {
        "AppName": "测试应用",           // 应用名称
        "AppKey": "你的AppKey",          // 应用标识
        "AppSecret": "你的AppSecret",     // 应用密钥
        "AppToken": "可选的Token",       // 可选的应用令牌
        "SignLifetime": 300              // 签名有效期（秒）
      }
    ]
  }
}
```

## 基本用法

1. 配置 AppKey/AppSecret
   * 在配置文件中添加 AppKey 和 AppSecret
   * 或者实现自定义的 `IAppKeyStore` 接口来管理 AppKey

2. 启用 OpenApi 认证
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       var configuration = context.Services.GetConfiguration();
       Configure<AbpOpenApiOptions>(configuration.GetSection("OpenApi"));
   }
   ```

3. 自定义客户端验证（可选）
   ```csharp
   public class CustomClientChecker : IClientChecker
   {
       public Task<bool> IsGrantAsync(string clientId, CancellationToken cancellationToken = default)
       {
           // 实现自定义的客户端验证逻辑
           return Task.FromResult(true);
       }
   }
   ```

4. 自定义IP地址验证（可选）
   ```csharp
   public class CustomIpAddressChecker : IIpAddressChecker
   {
       public Task<bool> IsGrantAsync(string ipAddress, CancellationToken cancellationToken = default)
       {
           // 实现自定义的IP地址验证逻辑
           return Task.FromResult(true);
       }
   }
   ```

## 错误代码

* AbpOpenApi:9100 - 无效的应用标识
* AbpOpenApi:9101 - 未携带应用标识
* AbpOpenApi:9110 - 无效的签名
* AbpOpenApi:9111 - 未携带签名
* AbpOpenApi:9210 - 请求超时或会话已过期
* AbpOpenApi:9211 - 未携带时间戳标识
* AbpOpenApi:9220 - 重复发起的请求
* AbpOpenApi:9221 - 未携带随机数
* AbpOpenApi:9300 - 客户端不在允许的范围内
* AbpOpenApi:9400 - 客户端IP不在允许的范围内

## 更多信息

* [ABP框架](https://abp.io/)
