# LINGYUN.Abp.Notifications.Domain.Shared

通知系统的共享领域层模块，提供了通知系统的共享常量、枚举和其他领域对象。

## 功能特性

* 通知类型定义
* 通知严重程度定义
* 通知状态定义
* 通知生命周期定义
* 通知常量定义

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsDomainSharedModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 枚举定义

### NotificationType

* Application - 应用程序通知
* System - 系统通知
* User - 用户通知

### NotificationSeverity

* Info - 信息
* Success - 成功
* Warn - 警告
* Error - 错误
* Fatal - 致命错误

### NotificationLifetime

* Persistent - 持久化通知
* OnlyOne - 一次性通知

## 基本用法

1. 使用通知类型
```csharp
var notificationType = NotificationType.Application;
```

2. 使用通知严重程度
```csharp
var severity = NotificationSeverity.Info;
```

## 更多信息

* [ABP文档](https://docs.abp.io)
