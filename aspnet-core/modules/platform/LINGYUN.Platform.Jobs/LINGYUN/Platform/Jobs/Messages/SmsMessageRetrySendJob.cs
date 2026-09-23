using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Platform.Messages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;

namespace LINGYUN.Platform.Jobs.Messages;
/// <summary>
/// 短信重新发送作业
/// </summary>
[DisableAuditing]
[DisableJobAction]
public class SmsMessageRetrySendJob : IJobRunnable
{
    public const string Name = "SmsMessageRetrySendJob";

    #region Definition Paramters

    public readonly static IReadOnlyList<JobDefinitionParamter> Paramters =
        new List<JobDefinitionParamter>
        {
            new JobDefinitionParamter(
                PropertyRecentSnedFailedHours,
                LocalizableStatic.Create("DisplayName:RecentSnedFailedHours"),
                LocalizableStatic.Create("Description:RecentSnedFailedHours")),
            new JobDefinitionParamter(
                PropertyMaxOfFailedAttempts,
                LocalizableStatic.Create("DisplayName:PropertyMaxOfFailedAttempts"),
                LocalizableStatic.Create("Description:PropertyMaxOfFailedAttempts")),
            new JobDefinitionParamter(
                PropertyPollingBatchCount,
                LocalizableStatic.Create("DisplayName:PropertyPollingBatchCount"),
                LocalizableStatic.Create("Description:PropertyPollingBatchCount")),
        };

    #endregion
    /// <summary>
    /// 最近发送失败时间, 单位: 小时, 默认: 2
    /// </summary>
    public const string PropertyRecentSnedFailedHours = "RecentSnedFailedHours";
    /// <summary>
    /// 失败次数上限, 默认: 3
    /// </summary>
    public const string PropertyMaxOfFailedAttempts = "MaxOfFailedAttempts";
    /// <summary>
    /// 轮询批次数量, 默认: 100
    /// </summary>
    public const string PropertyPollingBatchCount = "PollingBatchCount";

    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        var logger = context.GetRequiredService<ILogger<SmsMessageRetrySendJob>>();

        var distributedLock = context.GetRequiredService<IAbpDistributedLock>();
        var distributedLockKey = nameof(SmsMessageRetrySendJob);
        await using var jobLockHandle = await distributedLock.TryAcquireAsync(distributedLockKey);
        if (jobLockHandle == null)
        {
            logger.LogWarning("Handle is null because of the locking for : {distributedLockKey}", distributedLockKey);
            return;
        }

        logger.LogDebug("Lock is acquired for {distributedLockKey}.", distributedLockKey);

        var clock = context.GetRequiredService<IClock>();
        var smsMessageManager = context.GetRequiredService<ISmsMessageManager>();
        var smsMessageRepo = context.GetRequiredService<ISmsMessageRepository>();

        var recentSnedFailedHours = context.GetOrDefaultJobData(PropertyRecentSnedFailedHours, 2);
        var maxOfFailedAttempts = context.GetOrDefaultJobData(PropertyMaxOfFailedAttempts, 3);
        var pollingBatchCount = context.GetOrDefaultJobData(PropertyPollingBatchCount, 100);
        var sendTime = clock.Now.AddHours(-recentSnedFailedHours);

        var recentSnedFailedMessages = await smsMessageRepo.GetListAsync(
            new ExpressionSpecification<SmsMessage>(x => x.Status == MessageStatus.Failed &&
                x.SendCount < maxOfFailedAttempts && x.SendTime >= sendTime),
            maxResultCount: pollingBatchCount);

        if (recentSnedFailedMessages.Count > 0)
        {
            logger.LogDebug("In the last {Hour} hours, a total of {Count} SMS messages need to be resent.", recentSnedFailedHours, recentSnedFailedMessages.Count);

            foreach (var message in recentSnedFailedMessages)
            {
                await smsMessageManager.SendAsync(message);
            }

            await smsMessageRepo.UpdateManyAsync(recentSnedFailedMessages);
        }
        else
        {
            logger.LogDebug($"There are no SMS messages that need to be resent.");
        }

        logger.LogDebug($"The batch retry operation of the SMS messages has been successfully completed.");
    }
}
