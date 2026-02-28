using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.MessageService.Groups;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.MessageService;

[ConnectionStringName("Default")]
public class MessageServiceMigrationsDbContext : 
    AbpDbContext<MessageServiceMigrationsDbContext>,
    INotificationsDbContext,
    INotificationsDefinitionDbContext,
    IMessageServiceDbContext
{
    public DbSet<UserMessage> UserMessages { get; set; }

    public DbSet<GroupMessage> GroupMessages { get; set; }

    public DbSet<UserChatFriend> UserChatFriends { get; set; }

    public DbSet<UserChatSetting> UserChatSettings { get; set; }

    public DbSet<GroupChatBlack> GroupChatBlacks { get; set; }

    public DbSet<ChatGroup> ChatGroups { get; set; }

    public DbSet<UserChatGroup> UserChatGroups { get; set; }

    public DbSet<UserChatCard> UserChatCards { get; set; }

    public DbSet<UserGroupCard> UserGroupCards { get; set; }

    public DbSet<NotificationDefinitionGroupRecord> NotificationDefinitionGroupRecords { get; set; }

    public DbSet<NotificationDefinitionRecord> NotificationDefinitionRecords { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<UserNotification> UserNotifications { get; set; }

    public DbSet<UserSubscribe> UserSubscribes { get; set; }

    public MessageServiceMigrationsDbContext(DbContextOptions<MessageServiceMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureNotifications();
        modelBuilder.ConfigureNotificationsDefinition();
        modelBuilder.ConfigureMessageService();
    }
}
