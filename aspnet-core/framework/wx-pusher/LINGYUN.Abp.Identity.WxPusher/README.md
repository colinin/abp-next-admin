# LINGYUN.Abp.Identity.WxPusher

IWxPusherUserStore 接口的Identity模块实现, 通过用户Claims来获取关注的topic列表  

[English](./README.EN.md)

## 功能特性

* 集成WxPusher用户存储接口
* 通过用户Claims管理WxPusher的UID和Topic
* 支持批量获取用户绑定的UID
* 支持批量获取用户订阅的Topic

## 安装

```bash
dotnet add package LINGYUN.Abp.Identity.WxPusher
```

## 模块引用

```csharp
[DependsOn(typeof(AbpIdentityWxPusherModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 使用方式

该模块实现了 `IWxPusherUserStore` 接口，通过用户Claims存储WxPusher相关信息：

* `AbpWxPusherClaimTypes.Uid`: 存储用户绑定的WxPusher UID
* `AbpWxPusherClaimTypes.Topic`: 存储用户订阅的Topic ID

### 获取用户绑定的UID

```csharp
public class YourService
{
    private readonly IWxPusherUserStore _wxPusherUserStore;

    public YourService(IWxPusherUserStore wxPusherUserStore)
    {
        _wxPusherUserStore = wxPusherUserStore;
    }

    public async Task DoSomethingAsync(IEnumerable<Guid> userIds)
    {
        var uids = await _wxPusherUserStore.GetBindUidsAsync(userIds);
        // 使用获取到的uids进行消息推送等操作
    }
}
```

### 获取用户订阅的Topic

```csharp
public class YourService
{
    private readonly IWxPusherUserStore _wxPusherUserStore;

    public YourService(IWxPusherUserStore wxPusherUserStore)
    {
        _wxPusherUserStore = wxPusherUserStore;
    }

    public async Task DoSomethingAsync(IEnumerable<Guid> userIds)
    {
        var topics = await _wxPusherUserStore.GetSubscribeTopicsAsync(userIds);
        // 使用获取到的topics进行消息推送等操作
    }
}
```

## 源码位置

[LINGYUN.Abp.Identity.WxPusher](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/wx-pusher/LINGYUN.Abp.Identity.WxPusher)
