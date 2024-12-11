# LINGYUN.Abp.Notifications.EntityFrameworkCore

The EntityFrameworkCore module of the notification system, providing data access implementation for the notification system.

## Features

* Notification entity mapping configuration
* Notification repository implementation
* Notification subscription repository implementation
* Support for multiple databases
* Support for custom repository extensions

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Entity Mappings

### NotificationEfCoreEntityTypeConfiguration

* Notification - Notification entity mapping
  * Id - Primary key mapping
  * NotificationName - Notification name mapping
  * Data - Notification data mapping
  * CreationTime - Creation time mapping
  * Type - Notification type mapping
  * Severity - Notification severity mapping

### NotificationSubscriptionEfCoreEntityTypeConfiguration

* NotificationSubscription - Notification subscription entity mapping
  * UserId - User identifier mapping
  * NotificationName - Notification name mapping
  * CreationTime - Creation time mapping

## Basic Usage

1. Configure DbContext
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

2. Configure connection string
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=YourDb;Trusted_Connection=True"
  }
}
```

## More Information

* [ABP Documentation](https://docs.abp.io)
* [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core/)
