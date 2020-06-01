using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.MessageService.Subscriptions;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.MessageService.EntityFrameworkCore
{
    public static class MessageServiceDbContextModelCreatingExtensions
    {
        public static void ConfigureMessageService(
           this ModelBuilder builder,
           Action<MessageServiceModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new MessageServiceModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<Notification>(b =>
            {
                b.ToTable(options.TablePrefix + "Notifications", options.Schema);

                b.Property(p => p.NotificationName).HasMaxLength(NotificationConsts.MaxNameLength).IsRequired();
                b.Property(p => p.NotificationTypeName).HasMaxLength(NotificationConsts.MaxTypeNameLength).IsRequired();
                b.Property(p => p.NotificationData).HasMaxLength(NotificationConsts.MaxDataLength).IsRequired();

                b.ConfigureMultiTenant();
                b.ConfigureCreationTime();

                b.HasIndex(p => p.NotificationName);
            });

            builder.Entity<UserNotification>(b =>
            {
                b.ToTable(options.TablePrefix + "UserNotifications", options.Schema);

                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.UserId, p.NotificationId })
                .HasName("IX_Tenant_User_Notification_Id");
            });

            builder.Entity<UserSubscribe>(b =>
            {
                b.ToTable(options.TablePrefix + "UserSubscribes", options.Schema);

                b.Property(p => p.NotificationName).HasMaxLength(SubscribeConsts.MaxNotificationNameLength).IsRequired();

                b.ConfigureCreationTime();
                b.ConfigureMultiTenant();

                b.ConfigureMultiTenant();
                b.HasIndex(p => new { p.TenantId, p.UserId, p.NotificationName })
                .HasName("IX_Tenant_User_Notification_Name")
                .IsUnique();
            });
        }
    }
}
