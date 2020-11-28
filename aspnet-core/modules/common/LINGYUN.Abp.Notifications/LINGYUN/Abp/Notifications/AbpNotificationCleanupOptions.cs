namespace LINGYUN.Abp.Notifications
{
    public class AbpNotificationCleanupOptions
    {
        /// <summary>
        /// 是否启用清理任务
        /// 默认：启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>
        /// 清理时间间隔
        /// 默认：30_0000ms
        /// </summary>
        public int CleanupPeriod { get; set; } = 30_0000;
        /// <summary>
        /// 清理批次
        /// 默认： 200
        /// </summary>
        public int CleanupBatchSize { get; set; } = 200;
    }
}
