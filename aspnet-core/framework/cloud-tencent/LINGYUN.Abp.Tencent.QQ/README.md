# LINGYUN.Abp.Tencent.QQ

腾讯QQ互联模块，集成腾讯QQ互联服务到ABP应用程序。

## 功能特性

* 支持QQ互联快速登录
* 支持多租户配置
* 提供QQ互联客户端工厂，支持动态创建客户端

## 配置项说明

### 基础配置

```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "SecretId": "您的腾讯云SecretId", // 从腾讯云控制台获取
      "SecretKey": "您的腾讯云SecretKey", // 从腾讯云控制台获取
      "DurationSecond": "600" // 会话持续时间（秒）
    }
  }
}
```

### QQ互联配置

```json
{
  "Settings": {
    "Abp.TencentCloud.QQConnect": {
      "AppId": "", // QQ互联应用ID，从QQ互联管理中心获取
      "AppKey": "", // QQ互联应用密钥，从QQ互联管理中心获取
      "IsMobile": "false" // 是否使用移动端样式，默认为PC端样式
    }
  }
}
```

## 基本用法

1. 添加模块依赖
```csharp
[DependsOn(typeof(AbpTencentQQModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 配置QQ互联服务

参考上述配置项说明进行相应配置。

3. QQ互联服务使用示例
```csharp
public class YourService
{
    private readonly TencentQQClientFactory _qqClientFactory;

    public YourService(TencentQQClientFactory qqClientFactory)
    {
        _qqClientFactory = qqClientFactory;
    }

    public async Task QQConnectAsync()
    {
        var qqClient = await _qqClientFactory.CreateAsync();
        // 使用qqClient调用QQ互联服务API
        // 详细API使用方法请参考QQ互联开发文档
    }
}
```

## 更多文档

* [QQ互联开放平台](https://connect.qq.com/)
* [QQ互联开发文档](https://wiki.connect.qq.com/)

[English](./README.EN.md)
