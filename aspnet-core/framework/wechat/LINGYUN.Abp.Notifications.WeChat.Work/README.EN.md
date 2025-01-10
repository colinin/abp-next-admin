# LINGYUN.Abp.Notifications.WeChat.Work

WeChat Work (Enterprise WeChat) application message notification module, providing functionality to send messages to users through WeChat Work applications.

## Features

* Support WeChat Work application message sending
* Support text messages
* Support image messages
* Support voice messages
* Support video messages
* Support file messages
* Support news messages
* Support template card messages
* Integration with ABP notification system

## Module Reference

```csharp
[DependsOn(typeof(AbpNotificationsWeChatWorkModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WeChat": {
    "Work": {
      "Notifications": {
        "DefaultAgentId": 0,            // Default application ID
        "DefaultToParty": "",           // Default department ID list to receive messages
        "DefaultToTag": "",             // Default tag ID list to receive messages
        "DefaultSafe": 0,               // Default indicator for confidential messages, 0: shareable, 1: non-shareable with watermark
        "DefaultEnableIdTrans": 0,      // Default indicator for ID translation, 0: disabled, 1: enabled
        "DefaultEnableDuplicateCheck": 0, // Default indicator for duplicate message check, 0: disabled, 1: enabled
        "DefaultDuplicateCheckInterval": 1800 // Default interval for duplicate message check in seconds
      }
    }
  }
}
```

## More Documentation

* [Chinese Documentation](README.md)
