# LINGYUN.Abp.Notifications.WeChat.MiniProgram

WeChat Mini Program notification module, providing functionality to send subscription messages to users through WeChat Mini Program.

## Features

* Support WeChat Mini Program subscription message sending
* Support message template management
* Support dynamic message data configuration
* Integration with ABP notification system

## Module Reference

```csharp
[DependsOn(typeof(AbpNotificationsWeChatMiniProgramModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WeChat": {
    "MiniProgram": {
      "Notifications": {
        "DefaultTemplateId": "",           // Default template ID
        "DefaultPage": "pages/index/index", // Default jump page
        "DefaultState": "formal",          // Default Mini Program version type: developer/trial/formal
        "DefaultLang": "zh_CN"             // Default language
      }
    }
  }
}
```

## More Documentation

* [Chinese Documentation](README.md)
