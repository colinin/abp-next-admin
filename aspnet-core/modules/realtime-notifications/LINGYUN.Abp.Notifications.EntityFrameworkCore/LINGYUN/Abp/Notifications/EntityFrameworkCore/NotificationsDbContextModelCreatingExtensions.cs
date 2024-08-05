using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

public static class NotificationsDbContextModelCreatingExtensions
{
    public static void ConfigureNotifications(
       this ModelBuilder builder,
       Action<AbpNotificationsModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new AbpNotificationsModelBuilderConfigurationOptions();

        optionsAction?.Invoke(options);

        builder.Entity<Notification>(b =>
        {
            b.ToTable(options.TablePrefix + "Notifications", options.Schema);

            b.Property(p => p.NotificationName).HasMaxLength(NotificationConsts.MaxNameLength).IsRequired();
            b.Property(p => p.NotificationTypeName).HasMaxLength(NotificationConsts.MaxTypeNameLength).IsRequired();
            //b.Property(p => p.NotificationData).HasMaxLength(NotificationConsts.MaxDataLength).IsRequired();

            b.Property(p => p.ContentType)
             .HasDefaultValue(NotificationContentType.Text);

            b.ConfigureByConvention();

            b.HasIndex(p => new { p.TenantId, p.NotificationName });
        });

        builder.Entity<UserNotification>(b =>
        {
            b.ToTable(options.TablePrefix + "UserNotifications", options.Schema);

            b.ConfigureByConvention();

            b.HasIndex(p => new { p.TenantId, p.UserId, p.NotificationId })
             .HasDatabaseName("IX_Tenant_User_Notification_Id");
        });

        builder.Entity<UserSubscribe>(b =>
        {
            b.ToTable(options.TablePrefix + "UserSubscribes", options.Schema);

            b.Property(p => p.NotificationName).HasMaxLength(SubscribeConsts.MaxNotificationNameLength).IsRequired();
            b.Property(p => p.UserName)
                .HasMaxLength(SubscribeConsts.MaxUserNameLength)
                .HasDefaultValue("/")// 不是必须的
                .IsRequired();

            b.ConfigureByConvention();

            b.HasIndex(p => new { p.TenantId, p.UserId, p.NotificationName })
             .HasDatabaseName("IX_Tenant_User_Notification_Name")
             .IsUnique();
        });
    }

    public static void ConfigureNotificationsDefinition(
       this ModelBuilder builder,
       Action<AbpNotificationsModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new AbpNotificationsModelBuilderConfigurationOptions();

        optionsAction?.Invoke(options);

        builder.Entity<NotificationDefinitionGroupRecord>(b =>
        {
            b.ToTable(options.TablePrefix + "NotificationDefinitionGroups", options.Schema);
            b.Property(p => p.Name)
             .HasMaxLength(NotificationDefinitionGroupRecordConsts.MaxNameLength)
             .IsRequired();

            b.Property(p => p.DisplayName)
             .HasMaxLength(NotificationDefinitionGroupRecordConsts.MaxDisplayNameLength);
            b.Property(p => p.Description)
             .HasMaxLength(NotificationDefinitionGroupRecordConsts.MaxDescriptionLength);

            b.ConfigureByConvention();
        });

        builder.Entity<NotificationDefinitionRecord>(b =>
        {
            b.ToTable(options.TablePrefix + "NotificationDefinitions", options.Schema);
            b.Property(p => p.Name)
             .HasMaxLength(NotificationDefinitionRecordConsts.MaxNameLength)
             .IsRequired();
            b.Property(p => p.GroupName)
             .HasMaxLength(NotificationDefinitionGroupRecordConsts.MaxNameLength)
             .IsRequired();

            b.Property(p => p.DisplayName)
             .HasMaxLength(NotificationDefinitionRecordConsts.MaxDisplayNameLength);
            b.Property(p => p.Description)
             .HasMaxLength(NotificationDefinitionRecordConsts.MaxDescriptionLength);
            b.Property(p => p.Providers)
             .HasMaxLength(NotificationDefinitionRecordConsts.MaxProvidersLength);
            b.Property(p => p.Template)
             .HasMaxLength(NotificationDefinitionRecordConsts.MaxTemplateLength);

            b.Property(p => p.ContentType)
             .HasDefaultValue(NotificationContentType.Text);

            b.ConfigureByConvention();
        });
    }
}
