using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.RealtimeMessage.EntityFrameworkCore;

public class RealtimeMessageDbMigrationEventHandler : EfCoreDatabaseMigrationEventHandlerBase<RealtimeMessageMigrationsDbContext>
{
    protected INotificationSender NotificationSender { get; }
    protected INotificationSubscriptionManager NotificationSubscriptionManager { get; }

    public RealtimeMessageDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory,
        INotificationSender notificationSender,
        INotificationSubscriptionManager notificationSubscriptionManager)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<RealtimeMessageMigrationsDbContext>(), 
            currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        NotificationSender = notificationSender;
        NotificationSubscriptionManager = notificationSubscriptionManager;
    }

    protected async override Task AfterTenantCreated(TenantCreatedEto eventData, bool schemaMigrated)
    {
        if (!schemaMigrated)
        {
            return;
        }

        using (CurrentTenant.Change(eventData.Id))
        {
            await SendNotificationAsync(eventData);
        }
    }

    protected async virtual Task SendNotificationAsync(TenantCreatedEto eventData)
    {
        try
        {
            if (!eventData.Properties.TryGetValue("AdminUserId", out var userIdString) ||
                !Guid.TryParse(userIdString, out var adminUserId))
            {
                return;
            }
            var adminEmailAddress = eventData.Properties.GetOrDefault("AdminEmail") ?? "admin@abp.io";

            var tenantAdminUserIdentifier = new UserIdentifier(adminUserId, adminEmailAddress);

            // 租户管理员订阅事件
            await NotificationSubscriptionManager
                .SubscribeAsync(
                    eventData.Id,
                    tenantAdminUserIdentifier,
                    TenantNotificationNames.NewTenantRegistered);

            Logger.LogInformation("publish new tenant notification..");
            await NotificationSender.SendNofiterAsync(
                TenantNotificationNames.NewTenantRegistered,
                new NotificationTemplate(
                    TenantNotificationNames.NewTenantRegistered,
                    formUser: adminEmailAddress,
                    data: new Dictionary<string, object>
                    {
                            { "name", eventData.Name },
                            { "email", adminEmailAddress },
                            { "id", eventData.Id },
                    }),
                tenantAdminUserIdentifier,
                eventData.Id,
                NotificationSeverity.Success);

            Logger.LogInformation("tenant administrator subscribes to new tenant events..");
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to send the tenant initialization notification.");
        }
    }
}
