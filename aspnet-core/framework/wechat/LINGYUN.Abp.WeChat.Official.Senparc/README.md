# LINGYUN.Abp.WeChat.Official.Senparc

微信公众号Senparc SDK集成模块，提供与Senparc.Weixin SDK的集成支持。

## 功能特性

* Senparc.Weixin SDK集成
* 兼容Senparc消息处理机制
* 兼容Senparc事件处理机制
* 支持Senparc配置体系
* 支持Senparc缓存策略

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatOfficialSenparcModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "WeChat": {
    "Official": {
      "Senparc": {
        "IsEnabled": true,           // 是否启用Senparc集成
        "Cache": {
          "Type": "Local",          // 缓存类型：Local/Redis/Memcached
          "Configuration": ""        // 缓存配置字符串
        }
      }
    }
  }
}
```

## Senparc兼容性

本模块与Senparc.Weixin SDK保持兼容，可以：

* 使用Senparc的消息处理器
* 使用Senparc的事件处理器
* 使用Senparc的API调用方式
* 使用Senparc的缓存机制
* 使用Senparc的扩展功能

## 更多文档

* [微信公众号Senparc集成模块文档](README.EN.md)
* [Senparc.Weixin SDK文档](https://github.com/JeffreySu/WeiXinMPSDK)
