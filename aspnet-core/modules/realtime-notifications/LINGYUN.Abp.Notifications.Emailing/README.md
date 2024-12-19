# LINGYUN.Abp.Notifications.Emailing

通知系统的邮件发送模块，提供了通过邮件发送通知的功能。

## 功能特性

* 邮件通知发送
* 邮件模板支持
* 支持HTML格式邮件
* 支持多收件人
* 支持抄送和密送

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsEmailingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "Notifications": {
    "Emailing": {
      "Templates": {
        "Default": {
          "Template": "DefaultTemplate",
          "Culture": "zh-Hans"
        }
      }
    }
  }
}
```

## 基本用法

1. 配置邮件设置
```csharp
Configure<AbpEmailingOptions>(options =>
{
    options.DefaultFromAddress = "noreply@example.com";
    options.DefaultFromDisplayName = "Notification System";
});
```

2. 发送邮件通知
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

3. 使用邮件模板
```csharp
public async Task SendWithTemplateAsync()
{
    var template = await _templateRenderer.RenderAsync(
        "DefaultTemplate",
        new { 
            Title = "通知标题",
            Content = "通知内容"
        }
    );

    await _emailSender.SendAsync(
        to: "user@example.com",
        subject: "通知",
        body: template,
        isBodyHtml: true
    );
}
```

## 更多信息

* [ABP文档](https://docs.abp.io)
* [邮件发送文档](https://docs.abp.io/en/abp/latest/Emailing)
