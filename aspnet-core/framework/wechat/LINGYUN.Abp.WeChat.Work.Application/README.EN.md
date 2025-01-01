# LINGYUN.Abp.WeChat.Work.Application

WeChat Work application service module, providing application layer service implementation for WeChat Work application development.

## Features

* Contact management service
* Application management service
* Message pushing service
* Customer contact service
* Authentication service
* Enterprise payment service
* Electronic invoice service

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatWorkApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Application Services

### Contact Management

* `IWeChatWorkContactService`: Contact management service
  * Department management
  * Member management
  * Tag management
  * Interconnected enterprise management

### Application Management

* `IWeChatWorkAgentService`: Application management service
  * Application creation
  * Application configuration
  * Application visibility settings
  * Application homepage settings

### Message Pushing

* `IWeChatWorkMessageService`: Message pushing service
  * Text messages
  * Image messages
  * Voice messages
  * Video messages
  * File messages
  * Text card messages
  * News messages
  * Template card messages

## More Documentation

* [Chinese Documentation](README.md)
