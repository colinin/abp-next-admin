# LINGYUN.Abp.Notifications.WeChat.Work

WeChat Work notification publishing module, providing functionality to send messages to users through WeChat Work applications.

## Features

* Support for multiple message types
  * Text messages
  * Text card messages
  * Markdown messages
* Flexible message targeting
  * Send to specific users
  * Send to specific departments
  * Send to specific tags
  * Send to multiple applications
* Multi-language support
  * Localized message content
  * Multi-language titles and descriptions
* Feature toggle control
  * Message sending controlled by feature switches
* Extensibility
  * Custom notification definitions
  * Custom message handling

## Module Reference

```csharp
[DependsOn(typeof(AbpNotificationsWeChatWorkModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

### 1. Basic Configuration

Message sending requires configuring applications in the WeChat Work backend and obtaining the corresponding application ID (AgentId).

### 2. Notification Definition Configuration

```csharp
public class YourNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var notification = context.Create(
            name: "App.Notification.Test",
            displayName: L("TestNotification"))
            .WithAgentId("1000001")               // Set application ID
            .WithParty("1|2|3")                   // Set receiving departments
            .WithTag("TagId1|TagId2")             // Set receiving tags
            .WithAllAgent();                      // Send to all applications
    }
}
```

### 3. Sending Notifications

```csharp
public class YourService
{
    private readonly INotificationPublisher _notificationPublisher;

    public YourService(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task SendNotificationAsync()
    {
        var notificationData = new NotificationData();
        
        // Set message content
        notificationData.TrySetData("title", "Message Title");
        notificationData.TrySetData("message", "Message Content");
        notificationData.TrySetData("description", "Message Description");

        // Set sending targets
        notificationData.SetAgentId("1000001");   // Set application ID
        notificationData.SetParty("1|2|3");       // Set receiving departments
        notificationData.SetTag("TagId1|TagId2"); // Set receiving tags
        notificationData.WithAllAgent();          // Send to all applications

        await _notificationPublisher.PublishAsync(
            "App.Notification.Test",              // Notification name
            notificationData,                     // Notification data
            userIds: new[] { "userId" },          // Recipient user IDs
            tenantIds: new[] { "tenantId" }       // Tenant IDs
        );
    }
}
```

## Important Notes

1. Ensure WeChat Work application is properly configured, including application ID and relevant permissions.
2. Department IDs and tag IDs must match those configured in the WeChat Work backend.
3. Multiple recipients (departments/tags) are separated by '|', with a maximum of 100.
4. Using `WithAllAgent()` will send messages to all configured applications.
5. Message sending depends on WeChat Work API, ensure network connectivity is stable.

## More Documentation

* [Chinese Documentation](README.md)
* [WeChat Work Application Message Development Documentation](https://developer.work.weixin.qq.com/document/path/90236)
