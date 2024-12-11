# LINGYUN.Abp.Identity.WxPusher

Implementation of the IWxPusherUserStore interface for the Identity module, retrieving subscribed topic lists through user Claims.

[简体中文](./README.md)

## Features

* Integration with WxPusher user storage interface
* Manage WxPusher UID and Topic through user Claims
* Support batch retrieval of user-bound UIDs
* Support batch retrieval of user-subscribed Topics

## Installation

```bash
dotnet add package LINGYUN.Abp.Identity.WxPusher
```

## Module Reference

```csharp
[DependsOn(typeof(AbpIdentityWxPusherModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Usage

This module implements the `IWxPusherUserStore` interface, storing WxPusher-related information through user Claims:

* `AbpWxPusherClaimTypes.Uid`: Stores the WxPusher UID bound to the user
* `AbpWxPusherClaimTypes.Topic`: Stores the Topic ID subscribed by the user

### Get User-Bound UIDs

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
        // Use the retrieved uids for message pushing or other operations
    }
}
```

### Get User-Subscribed Topics

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
        // Use the retrieved topics for message pushing or other operations
    }
}
```

## Source Code

[LINGYUN.Abp.Identity.WxPusher](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/wx-pusher/LINGYUN.Abp.Identity.WxPusher)
