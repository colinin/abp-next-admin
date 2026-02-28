//using LINGYUN.Abp.BackgroundTasks;
//using LINGYUN.Abp.Identity.Notifications;
//using LINGYUN.Abp.Notifications;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Volo.Abp.Domain.Repositories;
//using Volo.Abp.Identity;
//using Volo.Abp.Specifications;
//using Volo.Abp.Timing;

//namespace LINGYUN.Abp.Identity.Jobs;

///// <summary>
///// 用户清理作业
///// </summary>
///// <remarks>
///// 清理长期未登录用户
///// </remarks>
//public class InactiveIdentityUserCleanupJob : IJobRunnable
//{
//    public const string Name = "InactiveIdentityUserCleanupJob";

//    #region Definition Paramters

//    public readonly static IReadOnlyList<JobDefinitionParamter> Paramters =
//        new List<JobDefinitionParamter>
//        {
//            new JobDefinitionParamter(
//                PropertyUserInactiveCleanupDays,
//                LocalizableStatic.Create("DisplayName:UserInactiveCleanupDays"),
//                LocalizableStatic.Create("Description:UserInactiveCleanupDays")),
//            new JobDefinitionParamter(
//                PropertyUserInactiveNotifierDays,
//                LocalizableStatic.Create("DisplayName:UserInactiveNotifierDays"),
//                LocalizableStatic.Create("Description:UserInactiveNotifierDays")),
//        };

//    #endregion
//    /// <summary>
//    /// 不活跃用户清理时间, 单位: 天
//    /// </summary>
//    public const string PropertyUserInactiveCleanupDays = "UserInactiveCleanupDays";
//    /// <summary>
//    /// 不活跃用户通知时间, 单位: 天
//    /// </summary>
//    public const string PropertyUserInactiveNotifierDays = "UserInactiveNotifierDays";

//    public async virtual Task ExecuteAsync(JobRunnableContext context)
//    {
//        var logger = context.GetRequiredService<ILogger<InactiveIdentityUserCleanupJob>>();

//        // 不活跃用户清理时间
//        var inactiveCleanupDays = context.GetOrDefaultJobData(PropertyUserInactiveCleanupDays, 30);
//        // 不活跃用户通知时间
//        var inactiveNotifierDays = context.GetOrDefaultJobData(PropertyUserInactiveNotifierDays, 180);

//        var clock = context.GetRequiredService<IClock>();
//        var identityUserRepo = context.GetRequiredService<IIdentityUserRepository>();
//        var identityUserSessionRepo = context.GetRequiredService<IIdentitySessionRepository>();

//        // 获取需要清理的用户集合
//        var specification = new ExpressionSpecification<IdentitySession>(
//            x => x.LastAccessed <= clock.Now.AddDays(-inactiveNotifierDays));

//        using (identityUserSessionRepo.DisableTracking())
//        {
//            var inactiveUserCount = await identityUserSessionRepo.GetCountAsync(specification);
//            if (inactiveUserCount == 0)
//            {
//                logger.LogInformation("There are no inactive users to be notified.");
//                return;
//            }
//            // 不活跃用户会话集合
//            var inactiveUsers = await identityUserSessionRepo.GetListAsync(specification, maxResultCount: inactiveUserCount);

//            // 直接清理的不活跃用户集合
//            var inactiveCleanupUsers = inactiveUsers.Where(x => x.LastAccessed <= clock.Now.AddDays(-inactiveCleanupDays));

//            // 需要通知的不活跃用户
//            var inactiveNotifierUsers = inactiveUsers.ExceptBy(inactiveCleanupUsers.Select(x => x.Id), x => x.Id);
//            if (inactiveNotifierUsers.Count() != 0)
//            {
//                await SendInactiveUserNotifier(context, inactiveNotifierUsers);
//            }

//            if (inactiveCleanupUsers.Count() != 0)
//            {
//                logger.LogInformation(
//                    "Prepare to clean up {count} users who have been inactive for more than {inactiveCleanupDays} days.",
//                    inactiveCleanupUsers.Count(),
//                    inactiveCleanupDays);

//                // 清理不活跃用户
//                await identityUserRepo.DeleteManyAsync(inactiveCleanupUsers.Select(x => x.UserId));
//            }
//        }
//        logger.LogInformation($"Cleaned inactive users.");
//    }

//    /// <summary>
//    /// 发送不活跃用户清理通知
//    /// </summary>
//    /// <param name="context"></param>
//    /// <param name="userSessions"></param>
//    /// <returns></returns>
//    private async Task SendInactiveUserNotifier(JobRunnableContext context, IEnumerable<IdentitySession> userSessions)
//    {
//        // TODO: 完成不活跃用户清理通知
//        var notificationSender = context.GetRequiredService<INotificationSender>();

//        var notificationTemplate = new NotificationTemplate(
//            IdentityNotificationNames.IdentityUser.CleaningUpInactiveUsers,
//            data: new Dictionary<string, object>
//            {

//            });

//        await notificationSender.SendNofiterAsync(
//            IdentityNotificationNames.IdentityUser.CleaningUpInactiveUsers,
//            notificationTemplate
//            );
//    }
//}
