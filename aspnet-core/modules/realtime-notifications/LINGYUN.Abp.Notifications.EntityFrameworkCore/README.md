# LINGYUN.Abp.Notifications.EntityFrameworkCore

通知系统的EntityFrameworkCore模块，提供了通知系统的数据访问实现。

## 功能特性

* 通知实体映射配置
* 通知仓储实现
* 通知订阅仓储实现
* 支持多数据库
* 支持自定义仓储扩展

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 实体映射

### NotificationEfCoreEntityTypeConfiguration

* Notification - 通知实体映射
  * Id - 主键映射
  * NotificationName - 通知名称映射
  * Data - 通知数据映射
  * CreationTime - 创建时间映射
  * Type - 通知类型映射
  * Severity - 通知严重程度映射

### NotificationSubscriptionEfCoreEntityTypeConfiguration

* NotificationSubscription - 通知订阅实体映射
  * UserId - 用户标识映射
  * NotificationName - 通知名称映射
  * CreationTime - 创建时间映射

## 基本用法

1. 配置DbContext
```csharp
public class YourDbContext : AbpDbContext<YourDbContext>
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureNotifications();
    }
}
```

2. 配置连接字符串
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=YourDb;Trusted_Connection=True"
  }
}
```

## 更多信息

* [ABP文档](https://docs.abp.io)
* [Entity Framework Core文档](https://docs.microsoft.com/ef/core/)
