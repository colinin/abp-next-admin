# LINGYUN.Abp.Elsa.Activities.Notifications

Elsa工作流的通知活动集成模块

## 功能

* 提供 **SendNotification** 活动用于发送通知
  * 支持发送给多个用户
  * 支持通知数据和模板内容
  * 支持JavaScript和Liquid语法
  * 支持设置通知严重程度
  * 集成ABP框架的 `INotificationSender` 接口
  * 支持多租户

## 配置使用

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesNotificationsModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "Notification": true    // 启用通知活动
    }
}
```

## 活动参数

* **NotificationName**: 注册的通知名称
* **NotificationData**: 通知数据或模板内容
* **To**: 接收用户ID列表
* **Severity**: 通知严重程度（默认为Info）

## 输出参数

* **NotificationId**: 发送的通知ID

[English](./README.EN.md)
