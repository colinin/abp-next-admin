# LINGYUN.Abp.Elsa.Activities.IM

Instant Messaging activities integration module for Elsa workflow

## Features

* Provides **SendMessage** activity for sending instant messages
  * Support sending user messages and group messages
  * Integration with ABP framework's `IMessageSender` interface
  * Support multi-tenancy

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesIMModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "IM": true    // Enable instant messaging activities
    }
}
```

## Activity Parameters

* **Content**: Message content
* **FormUser**: Sender user ID
* **FormUserName**: Sender username
* **To**: Recipient user ID (for user messages)
* **GroupId**: Recipient group ID (for group messages)

## Output Parameters

* **MessageId**: ID of the sent message

[中文文档](./README.md)
