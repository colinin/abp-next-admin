# LINGYUN.Abp.WeChat.Work.Common

WeChat Work common module, providing basic functionality implementation for WeChat Work application development.

## Features

* Unified message handling framework
* Unified message encryption/decryption mechanism
* Unified event handling mechanism
* Unified API calling interface
* Unified error handling mechanism
* AccessToken management
* JsApi Ticket management

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatWorkCommonModule))]
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
      "Common": {
        "MessageHandlerFactory": {
          "MessageResolvers": [],           // Message resolver list
          "MessageHandlers": []             // Message handler list
        },
        "AccessToken": {
          "CacheExpiration": 7200,         // AccessToken cache duration (seconds)
          "CacheKey": "WeChat:Work:AccessToken:{0}" // AccessToken cache key template
        },
        "JsApiTicket": {
          "CacheExpiration": 7200,         // JsApi Ticket cache duration (seconds)
          "CacheKey": "WeChat:Work:JsApiTicket:{0}" // JsApi Ticket cache key template
        }
      }
    }
  }
}
```

## Message Handling

* Support text message handling
* Support image message handling
* Support voice message handling
* Support video message handling
* Support location message handling
* Support link message handling
* Support event message handling

## Event Handling

* Support application menu events
* Support application entry events
* Support location reporting events
* Support async task completion events
* Support external contact events
* Support contact change events

## More Documentation

* [Chinese Documentation](README.md)
