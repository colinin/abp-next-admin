using Volo.Abp.BackgroundJobs;

namespace LINGYUN.Abp.MessageService.BackgroundJobs
{
    [BackgroundJobName("定时清理过期通知消息任务")]
    internal class NotificationCleanupExpritionJobArgs
    {
        /// <summary>
        /// 清理大小
        /// </summary>
        public int Count { get; set; }

        public NotificationCleanupExpritionJobArgs()
        {

        }

        public NotificationCleanupExpritionJobArgs(int count = 200)
        {
            Count = count;
        }
    }
}
