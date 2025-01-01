# LINGYUN.Abp.Notifications.Sms

通知发布提供程序的短信实现

大部分重写的模块都和官方模块名称保持一致,通过命名空间区分,主要是只改写了一小部分或者增加额外的功能
如果大部分模块代码都重写,或者完全就是扩展模块,才会定义自己的名字

#### 注意

自定义的发送方法可以通过实现 ##ISmsNotificationSender## 接口或重写 ##SmsNotificationSender## 即可

## 功能特性

* 短信通知发送
* 短信模板支持
* 支持多个短信服务商
* 支持短信变量替换
* 支持批量发送

## 配置使用

* 此配置项将在下一个短信相关大版本移除

```json

{
  "Notifications": {
    "Sms": {
      "TemplateParamsPrefix": "短信模板变量前缀"
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

## 基本用法

1. 实现短信发送接口
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

2. 注册短信发送服务
```csharp
Configure<AbpNotificationsSmsOptions>(options =>
{
    options.TemplateParamsPrefix = "sms_"; // 短信模板变量前缀
});
```

3. 发送短信通知
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
                // 短信模板参数
                ["sms_code"] = "123456",
                ["sms_time"] = "5"
            },
            userIds: new[] { CurrentUser.Id }
        );
    }
}
```

## 更多信息

* [ABP文档](https://docs.abp.io)
* [短信服务文档](https://docs.abp.io/en/abp/latest/SMS-Sending)
