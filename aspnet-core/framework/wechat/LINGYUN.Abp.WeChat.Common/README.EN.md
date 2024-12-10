# LINGYUN.Abp.WeChat.Common

WeChat common module, providing shared fundamental functionality implementation for WeChat product lines.

## Features

* Unified message handling framework
* Unified message encryption/decryption mechanism
* Unified event handling mechanism
* Unified API calling interface
* Unified error handling mechanism

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatCommonModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WeChat": {
    "Common": {
      "MessageHandlerFactory": {
        "MessageResolvers": [],           // Message resolver list
        "MessageHandlers": []             // Message handler list
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
* Support event message handling
* Support custom message handler extensions

## More Documentation

* [Chinese Documentation](README.md)

### Change Log
