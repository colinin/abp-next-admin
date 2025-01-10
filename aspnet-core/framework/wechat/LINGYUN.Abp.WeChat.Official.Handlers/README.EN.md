# LINGYUN.Abp.WeChat.Official.Handlers

WeChat Official Account message handling module, providing basic implementation for WeChat Official Account message and event handling.

## Features

* Text message handling
* Image message handling
* Voice message handling
* Video message handling
* Short video message handling
* Location message handling
* Link message handling
* Subscribe/Unsubscribe event handling
* QR code scan event handling
* Location report event handling
* Custom menu event handling
* Template message event handling

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatOfficialHandlersModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WeChat": {
    "Official": {
      "Handlers": {
        "DefaultResponseType": "text",  // Default response type: text/image/voice/video/music/news
        "DefaultResponse": "",          // Default response content
        "EnableDefaultResponse": true   // Whether to enable default response
      }
    }
  }
}
```

## Custom Handlers

To implement custom message handlers, inherit from the corresponding base class:

* Text message: `WeChatOfficialTextMessageHandlerBase`
* Image message: `WeChatOfficialImageMessageHandlerBase`
* Voice message: `WeChatOfficialVoiceMessageHandlerBase`
* Video message: `WeChatOfficialVideoMessageHandlerBase`
* Event message: `WeChatOfficialEventMessageHandlerBase`

## More Documentation

* [Chinese Documentation](README.md)
