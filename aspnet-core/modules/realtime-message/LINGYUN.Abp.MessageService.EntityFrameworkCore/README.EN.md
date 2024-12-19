# LINGYUN.Abp.MessageService.EntityFrameworkCore

EntityFrameworkCore implementation module for message service.

## Features

* Implements data access layer for message service
* Provides default repository implementations
* Implements repositories for the following entities:
  * ChatGroup
  * UserChatGroup
  * UserChatCard
  * UserChatSetting
  * UserChatFriend

## Dependencies

* [LINGYUN.Abp.MessageService.Domain](../LINGYUN.Abp.MessageService.Domain/README.EN.md)
* `AbpEntityFrameworkCoreModule`

## Installation

1. First, install the LINGYUN.Abp.MessageService.EntityFrameworkCore package to your project:

```bash
dotnet add package LINGYUN.Abp.MessageService.EntityFrameworkCore
```

2. Add `AbpMessageServiceEntityFrameworkCoreModule` to your module's dependency list:

```csharp
[DependsOn(typeof(AbpMessageServiceEntityFrameworkCoreModule))]
public class YourModule : AbpModule
{
}
```

3. Add message service related DbSet to your DbContext:

```csharp
public class YourDbContext : AbpDbContext<YourDbContext>, IMessageServiceDbContext
{
    public DbSet<ChatGroup> ChatGroups { get; set; }
    public DbSet<UserChatGroup> UserChatGroups { get; set; }
    public DbSet<UserChatCard> UserChatCards { get; set; }
    public DbSet<UserChatSetting> UserChatSettings { get; set; }
    public DbSet<UserChatFriend> UserChatFriends { get; set; }
    
    public YourDbContext(DbContextOptions<YourDbContext> options) 
        : base(options)
    {
    }
}
```

## More

[中文文档](README.md)
