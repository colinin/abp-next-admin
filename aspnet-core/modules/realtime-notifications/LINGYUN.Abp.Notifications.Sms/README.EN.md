# LINGYUN.Abp.Notifications.Sms

SMS implementation of notification publishing provider

Most rewritten modules maintain the same name as the official modules and are distinguished by namespace, mainly because only a small part was rewritten or additional functionality was added.
If most of the module code is rewritten, or if it's a completely extended module, then it will have its own name.

#### Note

Custom sending methods can be implemented by implementing the ##ISmsNotificationSender## interface or overriding ##SmsNotificationSender##

## Features

* SMS notification sending
* SMS template support
* Support for multiple SMS service providers
* Support for SMS variable replacement
* Support for batch sending

## Configuration

* This configuration item will be removed in the next major SMS-related version

```json
{
  "Notifications": {
    "Sms": {
      "TemplateParamsPrefix": "SMS template variable prefix"
    }
  }
}
```

```csharp
[DependsOn(typeof(AbpNotificationsSmsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

1. Implement SMS sending interface
```csharp
public class YourSmsNotificationSender : SmsNotificationSender
{
    public override async Task SendAsync(NotificationInfo notification)
    {
        var templateParams = GetTemplateParams(notification);
        await SmsService.SendAsync(
            notification.UserPhoneNumber,
            notification.Title,
            templateParams
        );
    }
}
```

2. Register SMS sending service
```csharp
Configure<AbpNotificationsSmsOptions>(options =>
{
    options.TemplateParamsPrefix = "sms_"; // SMS template variable prefix
});
```

3. Send SMS notification
```csharp
public class YourService
{
    private readonly INotificationSender _notificationSender;

    public YourService(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task SendSmsNotificationAsync()
    {
        await _notificationSender.SendNofiterAsync(
            "YourNotification",
            new NotificationData
            {
                // SMS template parameters
                ["sms_code"] = "123456",
                ["sms_time"] = "5"
            },
            userIds: new[] { CurrentUser.Id }
        );
    }
}
```

## More Information

* [ABP Documentation](https://docs.abp.io)
* [SMS Service Documentation](https://docs.abp.io/en/abp/latest/SMS-Sending)
