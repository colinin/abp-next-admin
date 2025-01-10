# LINGYUN.Abp.Elsa.Activities.IM

Elsa工作流的即时消息活动集成模块

## 功能

* 提供 **SendMessage** 活动用于发送即时消息
  * 支持发送用户消息和群组消息
  * 集成ABP框架的 `IMessageSender` 接口
  * 支持多租户

## 配置使用

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
        "IM": true    // 启用即时消息活动
    }
}
```

## 活动参数

* **Content**: 消息内容
* **FormUser**: 发送者用户ID
* **FormUserName**: 发送者用户名
* **To**: 接收用户ID（用户消息）
* **GroupId**: 接收群组ID（群组消息）

## 输出参数

* **MessageId**: 发送的消息ID

[English](./README.EN.md)
