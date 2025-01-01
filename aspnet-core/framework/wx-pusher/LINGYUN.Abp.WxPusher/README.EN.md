# LINGYUN.Abp.WxPusher

ABP module integrating WxPusher WeChat push service, implementing WxPusher related API documentation and providing WxPusher capabilities.

[简体中文](./README.md)

For more details, see WxPusher documentation: https://wxpusher.dingliqc.com/docs/#/

## Features

* Integration with WxPusher API
* Support message pushing to users and Topics
* Support multiple message types (Text, HTML, Markdown)
* Support message sending limits and quota management
* Support QR code generation
* Support user subscription management

## Installation

```bash
dotnet add package LINGYUN.Abp.WxPusher
```

## Module Reference

```csharp
[DependsOn(typeof(AbpWxPusherModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Configuration

### Settings Configuration

* `WxPusher.Security.AppToken`: Application identity token. With APP_TOKEN, you can send messages to users of the corresponding application. Please keep it strictly confidential.

### Features Configuration

* `WxPusher`: WxPusher feature group
* `WxPusher.Enable`: Globally enable WxPusher
* `WxPusher.Message.Enable`: Globally enable WxPusher message channel
* `WxPusher.Message`: WxPusher message push
* `WxPusher.Message.SendLimit`: WxPusher message push limit count
* `WxPusher.Message.SendLimitInterval`: WxPusher message push limit interval (days)

## Usage

### Sending Messages

```csharp
public class YourService
{
    private readonly IWxPusherMessageSender _messageSender;

    public YourService(IWxPusherMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public async Task SendMessageAsync()
    {
        await _messageSender.SendAsync(
            content: "Hello, WxPusher!",
            summary: "Message Summary",
            contentType: MessageContentType.Text,
            topicIds: new List<int> { 1, 2 },  // Optional: Send to specific Topics
            uids: new List<string> { "UID1", "UID2" },  // Optional: Send to specific users
            url: "https://example.com"  // Optional: URL to jump to when clicking the message
        );
    }
}
```

### User Subscription

Implement the `IWxPusherUserStore` interface to manage user subscriptions:

```csharp
public class YourWxPusherUserStore : IWxPusherUserStore
{
    public async Task<List<string>> GetBindUidsAsync(
        IEnumerable<Guid> userIds, 
        CancellationToken cancellationToken = default)
    {
        // Implement logic to get WxPusher UIDs bound to users
    }

    public async Task<List<int>> GetSubscribeTopicsAsync(
        IEnumerable<Guid> userIds, 
        CancellationToken cancellationToken = default)
    {
        // Implement logic to get Topic list subscribed by users
    }
}
```

## Source Code

[LINGYUN.Abp.WxPusher](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/wx-pusher/LINGYUN.Abp.WxPusher)
