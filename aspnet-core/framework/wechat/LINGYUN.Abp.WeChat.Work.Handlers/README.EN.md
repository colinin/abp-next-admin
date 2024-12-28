# LINGYUN.Abp.WeChat.Work.Handlers

WeChat Work message handling module, providing basic implementation for WeChat Work message and event handling.

## Features

* Text message handling
* Image message handling
* Voice message handling
* Video message handling
* Location message handling
* Link message handling
* Event message handling
* Custom message handler extensions

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatWorkHandlersModule))]
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
      "Handlers": {
        "DefaultResponseType": "text",  // Default response type: text/image/voice/video/news
        "DefaultResponse": "",          // Default response content
        "EnableDefaultResponse": true   // Whether to enable default response
      }
    }
  }
}
```

## Custom Handlers

To implement custom message handlers, inherit from the corresponding base class:

* Text message: `WeChatWorkTextMessageHandlerBase`
* Image message: `WeChatWorkImageMessageHandlerBase`
* Voice message: `WeChatWorkVoiceMessageHandlerBase`
* Video message: `WeChatWorkVideoMessageHandlerBase`
* Location message: `WeChatWorkLocationMessageHandlerBase`
* Link message: `WeChatWorkLinkMessageHandlerBase`
* Event message: `WeChatWorkEventMessageHandlerBase`

## Event Handlers

Built-in event handlers include:

* Application menu event handler
* Application entry event handler
* Location reporting event handler
* Async task completion event handler
* External contact event handler
* Contact change event handler

## More Documentation

* [Chinese Documentation](README.md)
