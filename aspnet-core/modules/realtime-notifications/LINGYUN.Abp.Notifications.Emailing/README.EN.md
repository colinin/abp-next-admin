# LINGYUN.Abp.Notifications.Emailing

The email sending module of the notification system, providing functionality to send notifications via email.

## Features

* Email notification sending
* Email template support
* Support for HTML format emails
* Support for multiple recipients
* Support for CC and BCC

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsEmailingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "Notifications": {
    "Emailing": {
      "Templates": {
        "Default": {
          "Template": "DefaultTemplate",
          "Culture": "en"
        }
      }
    }
  }
}
```

## Basic Usage

1. Configure Email Settings
```csharp
Configure<AbpEmailingOptions>(options =>
{
    options.DefaultFromAddress = "noreply@example.com";
    options.DefaultFromDisplayName = "Notification System";
});
```

2. Send Email Notification
```csharp
public class YourNotificationHandler : INotificationHandler
{
    private readonly IEmailSender _emailSender;

    public YourNotificationHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task HandleAsync(NotificationInfo notification)
    {
        await _emailSender.SendAsync(
            to: notification.UserEmail,
            subject: notification.Title,
            body: notification.Content,
            isBodyHtml: true
        );
    }
}
```

3. Use Email Template
```csharp
public async Task SendWithTemplateAsync()
{
    var template = await _templateRenderer.RenderAsync(
        "DefaultTemplate",
        new { 
            Title = "Notification Title",
            Content = "Notification Content"
        }
    );

    await _emailSender.SendAsync(
        to: "user@example.com",
        subject: "Notification",
        body: template,
        isBodyHtml: true
    );
}
```

## More Information

* [ABP Documentation](https://docs.abp.io)
* [Emailing Documentation](https://docs.abp.io/en/abp/latest/Emailing)
