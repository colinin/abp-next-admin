# LINGYUN.Abp.WeChat

微信基础模块，提供微信应用开发的基础功能和配置。

## 功能特性

* 微信基础配置管理
* 统一的微信API调用接口
* 微信AccessToken管理
* 统一的错误处理机制

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "WeChat": {
    "BaseUrl": "https://api.weixin.qq.com",  // 微信API基础地址
    "DefaultTimeout": 30000,                  // 默认超时时间（毫秒）
    "RetryCount": 3,                         // 重试次数
    "RetryMilliseconds": 1000                // 重试间隔（毫秒）
  }
}
```

## Settings配置

* `WeChat.BaseUrl` : 微信API基础地址
* `WeChat.DefaultTimeout` : 默认超时时间
* `WeChat.RetryCount` : 重试次数
* `WeChat.RetryMilliseconds` : 重试间隔

## 更多文档

* [微信基础模块文档](README.EN.md)
