# LINGYUN.Abp.WeChat.Work.HttpApi

WeChat Work HTTP API module, providing HTTP API interface implementation for WeChat Work application development.

## Features

* Contact management API
* Application management API
* Message pushing API
* Customer contact API
* Authentication API
* Enterprise payment API
* Electronic invoice API
* WeChat Work callback interface

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatWorkHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Interfaces

### Contact Management

* POST /api/wechat/work/contact/department/create
* POST /api/wechat/work/contact/department/update
* DELETE /api/wechat/work/contact/department/{id}
* POST /api/wechat/work/contact/user/create
* POST /api/wechat/work/contact/user/update
* DELETE /api/wechat/work/contact/user/{id}

### Application Management

* GET /api/wechat/work/agent/{agentId}
* POST /api/wechat/work/agent/set
* GET /api/wechat/work/agent/list
* POST /api/wechat/work/agent/workbench/template

### Message Pushing

* POST /api/wechat/work/message/text
* POST /api/wechat/work/message/image
* POST /api/wechat/work/message/voice
* POST /api/wechat/work/message/video
* POST /api/wechat/work/message/file
* POST /api/wechat/work/message/textcard
* POST /api/wechat/work/message/news
* POST /api/wechat/work/message/templatecard

### Callback Interface

* POST /api/wechat/work/callback/{corpId}

## Permissions

* `WeChatWork.Contact`: Contact management
* `WeChatWork.Agent`: Application management
* `WeChatWork.Message`: Message management
* `WeChatWork.Customer`: Customer management
* `WeChatWork.Payment`: Enterprise payment
* `WeChatWork.Invoice`: Electronic invoice

## More Documentation

* [Chinese Documentation](README.md)
