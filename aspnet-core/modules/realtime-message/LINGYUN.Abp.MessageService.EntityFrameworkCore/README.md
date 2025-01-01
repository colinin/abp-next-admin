# LINGYUN.Abp.MessageService.EntityFrameworkCore

消息服务的 EntityFrameworkCore 实现模块。

## 功能特性

* 实现消息服务的数据访问层
* 提供默认仓储实现
* 实现以下实体的仓储：
  * ChatGroup - 聊天组
  * UserChatGroup - 用户聊天组
  * UserChatCard - 用户聊天卡片
  * UserChatSetting - 用户聊天设置
  * UserChatFriend - 用户聊天好友

## 依赖模块

* [LINGYUN.Abp.MessageService.Domain](../LINGYUN.Abp.MessageService.Domain/README.md)
* `AbpEntityFrameworkCoreModule`

## 配置使用

1. 首先，需要安装 LINGYUN.Abp.MessageService.EntityFrameworkCore 到你的项目中：

```bash
dotnet add package LINGYUN.Abp.MessageService.EntityFrameworkCore
```

2. 添加 `AbpMessageServiceEntityFrameworkCoreModule` 到你的模块的依赖列表：

```csharp
[DependsOn(typeof(AbpMessageServiceEntityFrameworkCoreModule))]
public class YourModule : AbpModule
{
}
```

3. 在你的 DbContext 中添加消息服务相关的 DbSet：

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

## 更多

[English document](README.EN.md)
