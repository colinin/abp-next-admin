# LINGYUN.Abp.WeChat.Work.Application.Contracts

WeChat Work application service contracts module, providing application layer service interface definitions for WeChat Work application development.

## Features

* Contact management service interfaces
* Application management service interfaces
* Message pushing service interfaces
* Customer contact service interfaces
* Authentication service interfaces
* Enterprise payment service interfaces
* Electronic invoice service interfaces

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatWorkApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Service Interfaces

### Contact Management

* `IWeChatWorkContactService`
  * `CreateDepartmentAsync`: Create department
  * `UpdateDepartmentAsync`: Update department
  * `DeleteDepartmentAsync`: Delete department
  * `CreateUserAsync`: Create member
  * `UpdateUserAsync`: Update member
  * `DeleteUserAsync`: Delete member
  * `CreateTagAsync`: Create tag
  * `UpdateTagAsync`: Update tag
  * `DeleteTagAsync`: Delete tag

### Application Management

* `IWeChatWorkAgentService`
  * `GetAgentAsync`: Get application
  * `SetAgentAsync`: Set application
  * `GetAgentListAsync`: Get application list
  * `SetWorkbenchTemplateAsync`: Set workbench template

### Message Pushing

* `IWeChatWorkMessageService`
  * `SendTextAsync`: Send text message
  * `SendImageAsync`: Send image message
  * `SendVoiceAsync`: Send voice message
  * `SendVideoAsync`: Send video message
  * `SendFileAsync`: Send file message
  * `SendTextCardAsync`: Send text card message
  * `SendNewsAsync`: Send news message
  * `SendTemplateCardAsync`: Send template card message

## Permissions

* `WeChatWork.Contact`: Contact management
* `WeChatWork.Agent`: Application management
* `WeChatWork.Message`: Message management
* `WeChatWork.Customer`: Customer management
* `WeChatWork.Payment`: Enterprise payment
* `WeChatWork.Invoice`: Electronic invoice

## More Documentation

* [Chinese Documentation](README.md)
