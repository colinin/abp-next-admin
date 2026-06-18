using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.Identity.Notifications;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Specifications;
using Volo.Abp.TextTemplating;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Identity.Jobs;

/// <summary>
/// 不活跃用户清理作业
/// </summary>
/// <remarks>
/// 清理长期不活跃用户
/// </remarks>
public class InactiveIdentityUserCleanupJob : IJobRunnable
{
    public const string Name = "InactiveIdentityUserCleanupJob";

    #region Definition Paramters

    public readonly static IReadOnlyList<JobDefinitionParamter> Paramters =
        new List<JobDefinitionParamter>
        {
            new JobDefinitionParamter(
                PropertyInactivityPeriodDays,
                LocalizableStatic.Create("DisplayName:InactivityPeriodDays"),
                LocalizableStatic.Create("Description:InactivityPeriodDays")),
            new JobDefinitionParamter(
                PropertyGracePeriodDays,
                LocalizableStatic.Create("DisplayName:GracePeriodDays"),
                LocalizableStatic.Create("Description:GracePeriodDays")),
        };

    #endregion
    /// <summary>
    /// 停用不活跃用户天数
    /// </summary>
    public const string PropertyInactivityPeriodDays = "InactivityPeriodDays";
    /// <summary>
    /// 已停用用户宽限天数
    /// </summary>
    public const string PropertyGracePeriodDays = "GracePeriodDays";

    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        await Task.WhenAll([
            SetInactiveIdentityUsers(context),
            DeleteInactiveIdentityUsers(context)
        ]);
    }

    protected async virtual Task SetInactiveIdentityUsers(JobRunnableContext context)
    {
        // 不活跃用户停用时间: 用户距离上次登录天数 + 宽限天数
        var inactivityPeriodDays = context.GetOrDefaultJobData(PropertyInactivityPeriodDays, 180);
        var gracePeriodDays = context.GetOrDefaultJobData(PropertyGracePeriodDays, 60);

        var clock = context.GetRequiredService<IClock>();
        var logger = context.GetRequiredService<ILogger<InactiveIdentityUserCleanupJob>>();

        var identityUserRepo = context.GetRequiredService<IIdentityUserRepository>();
        var identityUserInactiveRepo = context.GetRequiredService<IIdentityUserInactiveRepository>();

        var specification = new ExpressionSpecification<IdentityUserInactive>(
            x => x.Status == InactiveUserStatus.Notified && x.LastSignInTime < clock.Now.AddDays(-(inactivityPeriodDays + gracePeriodDays)));

        var inactiveUserCount = await identityUserInactiveRepo.GetCountAsync(specification); 
        if (inactiveUserCount == 0)
        {
            logger.LogInformation("There are no inactive users that can be deactivated.");
            return;
        }

        var inactiveUserBatch = 1000;

        while (true)
        {
            var identityUserInactives = await identityUserInactiveRepo.GetListAsync(
                specification,
                maxResultCount: inactiveUserBatch);
            if (identityUserInactives.Count == 0)
            {
                logger.LogInformation("There are no inactive users that can be deactivated.");
                break;
            }

            var inactiveUsers = await identityUserRepo.GetListByIdsAsync(
                identityUserInactives.Select(x => x.UserId));
            if (inactiveUsers.Count > 0)
            {
                inactiveUsers.ForEach((inactiveUser) =>
                {
                    inactiveUser.SetIsActive(false);
                });

                await identityUserRepo.UpdateManyAsync(inactiveUsers);

                await SendInactiveUserDeactivationNotifier(context, inactiveUsers);
            }

            identityUserInactives.ForEach((identityUserInactive) =>
            {
                identityUserInactive.MarkDeactivated(clock.Now);
            });

            await identityUserInactiveRepo.UpdateManyAsync(identityUserInactives);

            if (identityUserInactives.Count < inactiveUserBatch)
            {
                break;
            }
        }

        logger.LogInformation("The operation to deactivate inactive users has been successfully completed.");
    }

    /// <summary>
    /// 发送不活跃用户停用通知
    /// </summary>
    /// <param name="context"></param>
    /// <param name="inactiveUsers"></param>
    /// <returns></returns>
    protected async virtual Task SendInactiveUserDeactivationNotifier(JobRunnableContext context, IEnumerable<IdentityUser> inactiveUsers)
    {
        var clock = context.GetRequiredService<IClock>();
        var notificationSender = context.GetRequiredService<INotificationSender>();

        var inactivityPeriodDays = context.GetOrDefaultJobData(PropertyInactivityPeriodDays, 180);
        var gracePeriodDays = context.GetOrDefaultJobData(PropertyGracePeriodDays, 60);

        var reactivateDate = clock.Now.AddDays(gracePeriodDays);
        await Parallel.ForEachAsync(
            inactiveUsers,
            context.CancellationToken,
            async (inactiveUser, ctx) =>
            {
                var inactivityDays = (int)(clock.Now - (inactiveUser.LastSignInTime ?? inactiveUser.CreationTime)).TotalDays;
                var notificationTemplateData = new Dictionary<string, object>
                {
                    { "now", clock.Now },
                    { "name", inactiveUser.Name },
                    { "userName", inactiveUser.UserName },
                    { "surname", inactiveUser.Surname },
                    { "email", inactiveUser.Email },
                    { "isActive", inactiveUser.IsActive },
                    { "inactivityDays", inactivityDays },
                    { "reactivateDate", reactivateDate },
                    { "creationTime", clock.Normalize(inactiveUser.CreationTime) },
                };
                if (inactiveUser.LastSignInTime.HasValue)
                {
                    notificationTemplateData["lastSignInTime"] = clock.Normalize(inactiveUser.LastSignInTime.Value.DateTime);
                }
                var notificationTemplate = new NotificationTemplate(
                    IdentityNotificationNames.IdentityUser.InactiveUserDeactivationNotifier,
                    data: notificationTemplateData);

                await notificationSender.SendNofiterAsync(
                    IdentityNotificationNames.IdentityUser.InactiveUserDeactivationNotifier,
                    template: notificationTemplate,
                    user: new UserIdentifier(inactiveUser.Id, inactiveUser.UserName),
                    tenantId: inactiveUser.TenantId,
                    severity: NotificationSeverity.Warn);
            });
    }

    protected async virtual Task DeleteInactiveIdentityUsers(JobRunnableContext context)
    {
        // 不活跃用户删除时间: 距离上次发送不活跃通知天数 + 宽限天数
        var gracePeriodDays = context.GetOrDefaultJobData(PropertyGracePeriodDays, 60);

        var clock = context.GetRequiredService<IClock>();
        var logger = context.GetRequiredService<ILogger<InactiveIdentityUserCleanupJob>>();

        var identityUserRepo = context.GetRequiredService<IIdentityUserRepository>();
        var identityUserInactiveRepo = context.GetRequiredService<IIdentityUserInactiveRepository>();

        var specification = new ExpressionSpecification<IdentityUserInactive>(
            x => x.Status == InactiveUserStatus.Deactivated && x.DeactivatedTime < clock.Now.AddDays(-gracePeriodDays));

        var inactiveUserCount = await identityUserInactiveRepo.GetCountAsync(specification); if (inactiveUserCount == 0)
        {
            logger.LogInformation("There are no inactive users to delete.");
            return;
        }

        var inactiveUserBatch = 1000;

        while (true)
        {
            var identityUserInactives = await identityUserInactiveRepo.GetListAsync(
                specification,
                maxResultCount: inactiveUserBatch);
            if (identityUserInactives.Count == 0)
            {
                logger.LogInformation("There are no inactive users to delete.");
                break;
            }

            await identityUserInactiveRepo.DeleteManyAsync(identityUserInactives);

            var inactiveUsers = await identityUserRepo.GetListByIdsAsync(
                identityUserInactives.Select(x => x.UserId));
            if (inactiveUsers.Count > 0)
            {
                await identityUserRepo.DeleteManyAsync(inactiveUsers);

                await SendInactiveUserDeletionNotifier(context, inactiveUsers);
            }

            if (identityUserInactives.Count < inactiveUserBatch)
            {
                break;
            }
        }

        logger.LogInformation("The operation to deactivate inactive users has been successfully completed.");
    }

    /// <summary>
    /// 发送不活跃用户删除通知
    /// </summary>
    /// <param name="context"></param>
    /// <param name="inactiveUsers"></param>
    /// <returns></returns>
    protected async virtual Task SendInactiveUserDeletionNotifier(JobRunnableContext context, IEnumerable<IdentityUser> inactiveUsers)
    {
        var clock = context.GetRequiredService<IClock>();
        var emailSender = context.GetRequiredService<IEmailSender>();
        var templateRenderer = context.GetRequiredService<ITemplateRenderer>();
        var logger = context.GetRequiredService<ILogger<InactiveIdentityUserCleanupJob>>();
        var stringLocalizer = context.GetRequiredService<IStringLocalizer<IdentityResource>>();

        var emailTitle = stringLocalizer["InactiveUserDeletionNotifier"];
        var gracePeriodDays = context.GetOrDefaultJobData(PropertyGracePeriodDays, 60);
        var inactivityPeriodDays = context.GetOrDefaultJobData(PropertyInactivityPeriodDays, 180);

        await Parallel.ForEachAsync(
            inactiveUsers,
            context.CancellationToken,
            async (inactiveUser, ctx) =>
            {
                if (!inactiveUser.EmailConfirmed)
                {
                    logger.LogWarning("{userName} email has not been confirmed, skipping the account deletion notification sending.", inactiveUser.UserName);
                    return;
                }
                try
                {
                    var notificationTemplateData = new Dictionary<string, object>
                    {
                        { "now", clock.Now },
                        { "name", inactiveUser.Name },
                        { "userName", inactiveUser.UserName },
                        { "surname", inactiveUser.Surname },
                        { "email", inactiveUser.Email },
                        { "isActive", inactiveUser.IsActive },
                        { "inactivityPeriodDays", inactivityPeriodDays },
                        { "gracePeriodDays", gracePeriodDays },
                        { "creationTime", clock.Normalize(inactiveUser.CreationTime) },
                    };
                    if (inactiveUser.LastSignInTime.HasValue)
                    {
                        notificationTemplateData["lastSignInTime"] = clock.Normalize(inactiveUser.LastSignInTime.Value.DateTime);
                    }

                    var emailBody = await templateRenderer.RenderAsync(
                        IdentityNotificationNames.IdentityUser.InactiveUserDeletionNotifier,
                        notificationTemplateData);

                    await emailSender.SendAsync(
                        inactiveUser.Email,
                        emailTitle,
                        emailBody,
                        isBodyHtml: true);
                }
                catch (Exception ex)
                {
                    logger.LogWarning("Failure in sending the email for notifying the deletion of the user {userName}, error: {message}", inactiveUser.UserName, ex.Message);
                }
            });
    }
}
