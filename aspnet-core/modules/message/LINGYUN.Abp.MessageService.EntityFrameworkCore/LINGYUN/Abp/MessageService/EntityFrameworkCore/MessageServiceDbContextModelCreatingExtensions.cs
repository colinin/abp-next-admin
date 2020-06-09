using LINGYUN.Abp.MessageService.Messages;
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
                b.Property(p => p.UserName)
                    .HasMaxLength(SubscribeConsts.MaxUserNameLength)
                    .HasDefaultValue("/")// 不是必须的
                    .IsRequired();

                b.ConfigureCreationTime();
                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.UserId, p.NotificationName })
                .HasName("IX_Tenant_User_Notification_Name")
                .IsUnique();
            });

            builder.Entity<UserMessage>(b =>
            {
                b.ToTable(options.TablePrefix + "UserMessages", options.Schema);

                b.Property(p => p.SendUserName).HasMaxLength(MessageConsts.MaxSendUserNameLength).IsRequired();
                b.Property(p => p.Content).HasMaxLength(MessageConsts.MaxContentLength).IsRequired();

                b.ConfigureCreationTime();
                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.ReceiveUserId });
            });

            builder.Entity<GroupMessage>(b =>
            {
                b.ToTable(options.TablePrefix + "GroupMessages", options.Schema);

                b.Property(p => p.SendUserName).HasMaxLength(MessageConsts.MaxSendUserNameLength).IsRequired();
                b.Property(p => p.Content).HasMaxLength(MessageConsts.MaxContentLength).IsRequired();

                b.ConfigureCreationTime();
                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.GroupId });
            });

            builder.Entity<UserChatSetting>(b =>
            {
                b.ToTable(options.TablePrefix + "UserChatSettings", options.Schema);

                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.UserId });
            });

            builder.Entity<UserSpecialFocus>(b =>
            {
                b.ToTable(options.TablePrefix + "UserSpecialFocuss", options.Schema);

                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.UserId });
            });

            builder.Entity<UserChatBlack>(b =>
            {
                b.ToTable(options.TablePrefix + "UserChatBlacks", options.Schema);

                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.UserId });
            });

            builder.Entity<GroupChatBlack>(b =>
            {
                b.ToTable(options.TablePrefix + "GroupChatBlacks", options.Schema);

                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.GroupId });
            });

            builder.Entity<ChatGroupAdmin>(b =>
            {
                b.ToTable(options.TablePrefix + "ChatGroupAdmins", options.Schema);

                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.GroupId });
            });

            builder.Entity<ChatGroup>(b =>
            {
                b.ToTable(options.TablePrefix + "ChatGroups", options.Schema);

                b.Property(p => p.Name).HasMaxLength(ChatGroupConsts.MaxNameLength).IsRequired();

                b.Property(p => p.Tag).HasMaxLength(ChatGroupConsts.MaxTagLength);
                b.Property(p => p.Notice).HasMaxLength(ChatGroupConsts.MaxNoticeLength);
                b.Property(p => p.Address).HasMaxLength(ChatGroupConsts.MaxAddressLength);
                b.Property(p => p.Description).HasMaxLength(ChatGroupConsts.MaxDescriptionLength);

                b.ConfigureAudited();
                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.Name });
            });

            builder.Entity<UserChatGroup>(b =>
            {
                b.ToTable(options.TablePrefix + "UserChatGroups", options.Schema);

                b.ConfigureCreationAudited();
                b.ConfigureMultiTenant();

                b.HasIndex(p => new { p.TenantId, p.GroupId, p.UserId });
            });
        }
    }
}
